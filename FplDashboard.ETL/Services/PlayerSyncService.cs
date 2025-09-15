using FplDashboard.DataModel;
using FplDashboard.DataModel.Models;
using FplDashboard.ETL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FplDashboard.ETL.Services;

public class PlayerSyncService(FplDashboardDbContext database)
{
    public async Task SyncAsync(List<Player> playerList, CancellationToken cancellationToken)
    {
        var playerIds = playerList.Select(p => p.Id);
        var existingPlayers = await database.Players
                .Where(p => playerIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, cancellationToken);

        var playersToAdd = new List<Player>();
        foreach (var player in playerList)
        {
            if (existingPlayers.TryGetValue(player.Id, out var existingPlayer))
            {
                existingPlayer.CopyMutablePropertiesFrom(player);
            }
            else
            {
                playersToAdd.Add(player);
            }
        }
        await database.Players.AddRangeAsync(playersToAdd, cancellationToken);
    }
}
