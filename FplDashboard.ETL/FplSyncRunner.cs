using FplDashboard.DataModel;
using FplDashboard.ETL.Models;
using FplDashboard.ETL.Services;
using System.Text.Json;
using FplDashboard.ETL.Extensions;
using FplDashboard.ETL.Interfaces;

namespace FplDashboard.ETL;

public class FplSyncRunner(
    ILogger<FplSyncRunner> logger,
    IFplApiClient apiClient,
    FplDashboardDbContext database,
    TeamSyncService teamSync,
    GameWeekSyncService gameWeekSync,
    Services.FplSyncRunner fplSync,
    FplSyncRunnerTestsTeamGameWeekSyncService fplSyncRunnerTestsTeamGameWeekSync,
    FplSyncRunnerTestsPlayerGameWeekSyncService fplSyncRunnerTestsPlayerGameWeekSync,
    FplSyncRunnerTestsPlayerNewsSyncService fplSyncRunnerTestsPlayerNewsSync)
{
    public async Task RunSyncAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Fetching FPL data at: {time}", DateTimeOffset.Now);
        try
        {
            await using var transaction = await database.Database.BeginTransactionAsync(stoppingToken);
            var response = await apiClient.GetMainFplData(stoppingToken);
            var data = JsonSerializer.Deserialize<WrapperFromApi>(response);
            if (data is null)
            {
                logger.LogError("Failed to deserialize FPL data");
                return;
            }

            var gameWeeks = data.Events.FilterToRelevantEvents().Select(Event.GetGameWeekFromEtlModel).ToList();
            var currentGameWeekId = data.Events.GetCurrentGameWeekId();
            var teams = data.Teams.Select(Team.GetTeamDataModelFromEtlModel).ToList();
            var teamGameWeeks = data.Teams.Select(t => Team.GetTeamGameWeekDataFromEtlModel(t, currentGameWeekId)).ToList();
            var players = data.Elements.Select(PlayerElement.GetPlayerFromEtlModel).ToList();
            var playerGameWeeks = data.Elements.Select(p => PlayerElement.GetPlayerGameWeekDataFromEtlModel(p, currentGameWeekId)).ToList();
            var playerNews = data.Elements.Select(PlayerElement.GetPlayerNewsFromEtlModel).Where(n => n is not null).Cast<DataModel.Models.PlayerNews>().ToList();

            await teamSync.SyncAsync(teams, stoppingToken);
            await gameWeekSync.SyncAsync(gameWeeks, stoppingToken);
            await fplSync.SyncAsync(players, stoppingToken);
            await fplSyncRunnerTestsTeamGameWeekSync.SyncAsync(teamGameWeeks, currentGameWeekId, stoppingToken);
            await fplSyncRunnerTestsPlayerGameWeekSync.SyncAsync(playerGameWeeks, currentGameWeekId, stoppingToken);
            await fplSyncRunnerTestsPlayerNewsSync.SyncAsync(playerNews, stoppingToken);
            await database.SaveChangesAsync(stoppingToken);
            await transaction.CommitAsync(stoppingToken);
            logger.LogInformation("Finished fetching FPL data at: {time}", DateTimeOffset.Now);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching FPL data");
        }
    }
}
