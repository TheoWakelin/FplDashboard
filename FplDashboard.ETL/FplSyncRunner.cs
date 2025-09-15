using FplDashboard.DataModel;
using FplDashboard.ETL.Models;
using FplDashboard.ETL.Services;
using System.Text.Json;
using FplDashboard.ETL.Extensions;
using FplDashboard.ETL.Interfaces;
using Microsoft.EntityFrameworkCore;
using FixtureInDb = FplDashboard.DataModel.Models.Fixture;

namespace FplDashboard.ETL;

public class FplSyncRunner(
    ILogger<FplSyncRunner> logger,
    IFplApiClient apiClient,
    FplDashboardDbContext database,
    TeamSyncService teamSync,
    GameWeekSyncService gameWeekSync,
    Services.PlayerSyncService playerSync,
    TeamGameWeekSyncService teamGameWeekSync,
    PlayerGameWeekSyncService playerGameWeekSync,
    PlayerNewsSyncService playerNewsSync,
    FixtureSyncService fixtureSync)
{
    public async Task RunSyncAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Fetching FPL data at: {time}", DateTimeOffset.Now);
        try
        {
            await using var transaction = await database.Database.BeginTransactionAsync(stoppingToken);
            var mainFplDataTask = apiClient.GetMainFplData(stoppingToken);
            var fixturesDataTask = apiClient.GetFixturesData(stoppingToken);
            await Task.WhenAll(mainFplDataTask, fixturesDataTask);

            var data = JsonSerializer.Deserialize<WrapperFromApi>(mainFplDataTask.Result);
            if (data is null)
            {
                logger.LogError("Failed to deserialize FPL data");
                return;
            }

            var gameWeeks = data.Events.Select(Event.GetGameWeekFromEtlModel).ToList();
            var currentGameWeekId = data.Events.GetCurrentGameWeekId();
            var teams = data.Teams.Select(Team.GetTeamDataModelFromEtlModel).ToList();
            var teamGameWeeks = data.Teams.Select(t => Team.GetTeamGameWeekDataFromEtlModel(t, currentGameWeekId)).ToList();
            var players = data.Elements.Select(PlayerElement.GetPlayerFromEtlModel).ToList();
            var playerGameWeeks = data.Elements.Select(p => PlayerElement.GetPlayerGameWeekDataFromEtlModel(p, currentGameWeekId)).ToList();
            var playerNews = data.Elements.Select(PlayerElement.GetPlayerNewsFromEtlModel).Where(n => n is not null).Cast<DataModel.Models.PlayerNews>().ToList();
           
            await teamSync.SyncAsync(teams, stoppingToken);
            await gameWeekSync.SyncAsync(gameWeeks, stoppingToken);
            await playerSync.SyncAsync(players, stoppingToken);
            await teamGameWeekSync.SyncAsync(teamGameWeeks, currentGameWeekId, stoppingToken);
            await playerGameWeekSync.SyncAsync(playerGameWeeks, currentGameWeekId, stoppingToken);
            await playerNewsSync.SyncAsync(playerNews, stoppingToken);
            await database.SaveChangesAsync(stoppingToken);
            
            if (!string.IsNullOrWhiteSpace(fixturesDataTask.Result))
            {
                var fixtures = await MapFixturesFromApiResponse(fixturesDataTask.Result);
                await fixtureSync.SyncAsync(fixtures, currentGameWeekId, data.Events.GetPreviousGameWeekId(), stoppingToken);
                await database.SaveChangesAsync(stoppingToken);
            }
            
            await transaction.CommitAsync(stoppingToken);
            logger.LogInformation("Finished fetching FPL data at: {time}", DateTimeOffset.Now);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching FPL data");
        }
    }
    
    private async Task<List<FixtureInDb>> MapFixturesFromApiResponse(string fixturesResponse)
    {
        var fixturesEtl = JsonSerializer.Deserialize<List<Fixture>>(fixturesResponse);
        if (fixturesEtl == null) return [];
        
        // Build lookup from GameWeekNumber to Id
        var gameWeekLookup = await database.GameWeeks.ToDictionaryAsync(gw => gw.GameWeekNumber, gw => gw.Id);
        
        var mappedFixtures = new List<FixtureInDb>();
        foreach (var f in fixturesEtl)
        {
            if (!gameWeekLookup.TryGetValue(f.GameweekId, out var dbGameWeekId)) continue;
            var dbFixture = f.ToDataModelFixture(dbGameWeekId);
            mappedFixtures.Add(dbFixture);
        }
        return mappedFixtures;
    }
}
