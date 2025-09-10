using FplDashboard.DataModel;
using FplDashboard.DataModel.Models;
using Microsoft.EntityFrameworkCore;

namespace FplDashboard.ETL.Services;

public class FplSyncRunnerTestsPlayerNewsSyncService(FplDashboardDbContext database)
{
    public async Task SyncAsync(List<PlayerNews> playerNewsList, CancellationToken cancellationToken)
    {
        // Fetch the newest news date in the DB for any player.
        // Only add news that is newer than this date, as all older news is already present.
        var maxNewsDate = await database.PlayerNews.MaxAsync(n => n.NewsAdded, cancellationToken) ?? DateTime.MinValue;
        var newsToAdd = playerNewsList.Where(n => n.NewsAdded > maxNewsDate).ToList();
        await database.PlayerNews.AddRangeAsync(newsToAdd, cancellationToken);
    }
}
