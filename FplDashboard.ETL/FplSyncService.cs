using FplDashboard.DataModel;
using FplDashboard.DataModel.Models;
using FplDashboard.ETL.Extensions;
using FplDashboard.ETL.Models;
using FplDashboard.ETL.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Team = FplDashboard.ETL.Models.Team;

namespace FplDashboard.ETL;

public class FplSyncService(
    ILogger<FplSyncService> logger,
    IHttpClientFactory httpClientFactory,
    IServiceScopeFactory scopeFactory
) : BackgroundService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Fetching FPL data at: {time}", DateTimeOffset.Now);

            using var scope = scopeFactory.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<FplDashboardDbContext>();
            var teamSync = scope.ServiceProvider.GetRequiredService<TeamSyncService>();
            var gameWeekSync = scope.ServiceProvider.GetRequiredService<GameWeekSyncService>();
            var playerSync = scope.ServiceProvider.GetRequiredService<PlayerSyncService>();
            var teamGameWeekSync = scope.ServiceProvider.GetRequiredService<TeamGameWeekSyncService>();
            var playerGameWeekSync = scope.ServiceProvider.GetRequiredService<PlayerGameWeekSyncService>();
            var playerNewsSync = scope.ServiceProvider.GetRequiredService<PlayerNewsSyncService>();

            try
            {
                await using var transaction = await database.Database.BeginTransactionAsync(stoppingToken);

                var response = await _httpClient.GetStringAsync("https://fantasy.premierleague.com/api/bootstrap-static/", stoppingToken);
                var data = JsonSerializer.Deserialize<WrapperFromApi>(response);
                if (data is null)
                {
                    logger.LogError("Failed to deserialize FPL data");
                    break;
                }

                // Keep only previous, current, and next events to avoid unnecessary data processing and database saves.
                var gameWeeks = data.Events.FilterToRelevantEvents().Select(Event.GetGameWeekFromEtlModel).ToList();
                var currentGameWeekId = data.Events.GetCurrentGameWeekId();
                var teams = data.Teams.Select(Team.GetTeamDataModelFromEtlModel).ToList();
                var teamGameWeeks = data.Teams.Select(t => Team.GetTeamGameWeekDataFromEtlModel(t, currentGameWeekId)).ToList();
                var players = data.Elements.Select(PlayerElement.GetPlayerFromEtlModel).ToList();
                var playerGameWeeks = data.Elements.Select(p => PlayerElement.GetPlayerGameWeekDataFromEtlModel(p, currentGameWeekId)).ToList();
                var playerNews = data.Elements.Select(PlayerElement.GetPlayerNewsFromEtlModel).Where(n => n is not null).Cast<PlayerNews>().ToList();
                
                await teamSync.SyncAsync(teams, stoppingToken);
                await gameWeekSync.SyncAsync(gameWeeks, stoppingToken);
                await playerSync.SyncAsync(players, stoppingToken);
                await teamGameWeekSync.SyncAsync(teamGameWeeks, currentGameWeekId, stoppingToken);
                await playerGameWeekSync.SyncAsync(playerGameWeeks, currentGameWeekId, stoppingToken);
                await playerNewsSync.SyncAsync(playerNews, stoppingToken);

                await database.SaveChangesAsync(stoppingToken);
                await transaction.CommitAsync(stoppingToken);
                logger.LogInformation("Finished fetching FPL data at: {time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching FPL data");
            }

            await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
        }
    }
}
