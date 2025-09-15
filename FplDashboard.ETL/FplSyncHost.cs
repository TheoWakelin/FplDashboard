using FplDashboard.DataModel;
using FplDashboard.ETL.Interfaces;
using FplDashboard.ETL.Services;

namespace FplDashboard.ETL;

public class FplSyncHost(IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FplDashboardDbContext>();
            var teamSync = scope.ServiceProvider.GetRequiredService<TeamSyncService>();
            var gameWeekSync = scope.ServiceProvider.GetRequiredService<GameWeekSyncService>();
            var playerSync = scope.ServiceProvider.GetRequiredService<Services.PlayerSyncService>();
            var teamGameWeekSync = scope.ServiceProvider.GetRequiredService<TeamGameWeekSyncService>();
            var playerGameWeekSync = scope.ServiceProvider.GetRequiredService<PlayerGameWeekSyncService>();
            var playerNewsSync = scope.ServiceProvider.GetRequiredService<PlayerNewsSyncService>();
            var runnerLogger = scope.ServiceProvider.GetRequiredService<ILogger<FplSyncRunner>>();
            var fixtureSyncService = scope.ServiceProvider.GetRequiredService<FixtureSyncService>();
            var apiClient = scope.ServiceProvider.GetRequiredService<IFplApiClient>();
            var runner = new FplSyncRunner(
                runnerLogger,
                apiClient,
                db,
                teamSync,
                gameWeekSync,
                playerSync,
                teamGameWeekSync,
                playerGameWeekSync,
                playerNewsSync,
                fixtureSyncService
            );
            await runner.RunSyncAsync(stoppingToken);
            
            await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
        }
    }
}
