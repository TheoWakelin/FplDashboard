using FplDashboard.DataModel;
using FplDashboard.DataModel.Models;
using FplDashboard.ETL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FplDashboard.ETL.Services;

public class GameWeekSyncService(FplDashboardDbContext database)
{
    public async Task SyncAsync(List<GameWeek> gameWeekList, CancellationToken cancellationToken)
    {
        var existingGameWeeks = await database.GameWeeks
            .Where(gw => gameWeekList.Select(gwl => gwl.GameWeekNumber).Contains(gw.GameWeekNumber))
            .ToDictionaryAsync(gw => gw.GameWeekNumber, cancellationToken);

        var gameWeeksToAdd = new List<GameWeek>();
        foreach (var gameWeek in gameWeekList)
        {
            if (existingGameWeeks.TryGetValue(gameWeek.GameWeekNumber, out var existingGameWeek))
            {
                existingGameWeek.CopyMutablePropertiesFrom(gameWeek);
            }
            else
            {
                gameWeeksToAdd.Add(gameWeek);
            }
        }
        await database.GameWeeks.AddRangeAsync(gameWeeksToAdd, cancellationToken);
    }
}
