using FplDashboard.DataModel;
using FplDashboard.DataModel.Models;
using FplDashboard.ETL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FplDashboard.ETL.Services;

public class FplSyncRunnerTestsTeamGameWeekSyncService(FplDashboardDbContext database)
{
    public async Task SyncAsync(List<TeamGameWeekData> teamGameWeekDataList, int currentGameWeekId, CancellationToken cancellationToken)
    {
        var existingTeamGameWeekData = await database.TeamGameWeekData
            .Where(tg => tg.GameWeekId == currentGameWeekId)
            .ToDictionaryAsync(tg => tg.TeamId, cancellationToken);
        var teamGameWeeksToAdd = new List<TeamGameWeekData>();
        foreach (var teamGameWeek in teamGameWeekDataList)
        {
            if (existingTeamGameWeekData.TryGetValue(teamGameWeek.TeamId, out var existingTeamGameWeek))
            {
                existingTeamGameWeek.CopyMutablePropertiesFrom(teamGameWeek);
            }
            else
            {
                teamGameWeeksToAdd.Add(teamGameWeek);
            }
        }
        await database.TeamGameWeekData.AddRangeAsync(teamGameWeeksToAdd, cancellationToken);
    }
}
