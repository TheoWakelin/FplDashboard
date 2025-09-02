using FplDashboard.DataModel;
using FplDashboard.DataModel.Models;
using FplDashboard.ETL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FplDashboard.ETL.Services;

public class TeamSyncService(FplDashboardDbContext database)
{
    public async Task SyncAsync(List<Team> teamList, CancellationToken cancellationToken)
    {
        var existingTeams = await database.Teams.ToDictionaryAsync(t => t.Id, cancellationToken);
        var teamsToAdd = new List<Team>();
        foreach (var team in teamList)
        {
            if (existingTeams.TryGetValue(team.Id, out var existingTeam))
            {
                existingTeam.CopyMutablePropertiesFrom(team);
            }
            else
            {
                teamsToAdd.Add(team);
            }
        }
        await database.Teams.AddRangeAsync(teamsToAdd, cancellationToken);
    }
}
