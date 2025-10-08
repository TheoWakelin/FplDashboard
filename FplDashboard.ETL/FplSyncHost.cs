namespace FplDashboard.ETL;

public class FplSyncHost(IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<FplSyncRunner>();
            await runner.RunSyncAsync(stoppingToken);
            
            await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
        }
    }
}
