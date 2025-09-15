using FplDashboard.DataModel;
using FplDashboard.DataModel.Models;
using FplDashboard.ETL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FplDashboard.ETL.Services;

public class PlayerGameWeekSyncService(FplDashboardDbContext database)
{
    public async Task SyncAsync(List<PlayerGameWeekData> playerGameWeekDataList, int currentGameWeekId, CancellationToken cancellationToken)
    {
        var existingPlayerGameWeekData = await database.PlayerGameWeekData
            .Where(pg => pg.GameWeekId == currentGameWeekId)
            .ToDictionaryAsync(pg => pg.PlayerId, cancellationToken);

        var playerGameWeeksToAdd = new List<PlayerGameWeekData>();
        foreach (var playerGameWeek in playerGameWeekDataList)
        {
            if (existingPlayerGameWeekData.TryGetValue(playerGameWeek.PlayerId, out var existingPlayerGameWeek))
            {
                existingPlayerGameWeek.CopyMutablePropertiesFrom(playerGameWeek);
            }
            else
            {
                playerGameWeeksToAdd.Add(playerGameWeek);
            }
        }
        await database.PlayerGameWeekData.AddRangeAsync(playerGameWeeksToAdd, cancellationToken);
    }
}
