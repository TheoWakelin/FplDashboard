using FplDashboard.DataModel;
using FplDashboard.ETL.Interfaces;
using FplDashboard.ETL.Services;

namespace FplDashboard.ETL;

public class FplSyncHost(
    IFplApiClient apiClient,
    IServiceScopeFactory scopeFactory)
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
            var playerSync = scope.ServiceProvider.GetRequiredService<Services.FplSyncRunner>();
            var teamGameWeekSync = scope.ServiceProvider.GetRequiredService<FplSyncRunnerTestsTeamGameWeekSyncService>();
            var playerGameWeekSync = scope.ServiceProvider.GetRequiredService<FplSyncRunnerTestsPlayerGameWeekSyncService>();
            var playerNewsSync = scope.ServiceProvider.GetRequiredService<FplSyncRunnerTestsPlayerNewsSyncService>();
            var runnerLogger = scope.ServiceProvider.GetRequiredService<ILogger<FplSyncRunner>>();
            var runner = new FplSyncRunner(
                runnerLogger,
                apiClient,
                db,
                teamSync,
                gameWeekSync,
                playerSync,
                teamGameWeekSync,
                playerGameWeekSync,
                playerNewsSync
            );
            await runner.RunSyncAsync(stoppingToken);
        }
    }
}
