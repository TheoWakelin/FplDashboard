using FplDashboard.DataModel;
using FplDashboard.DataModel.Models;

namespace FplDashboard.ETL.IntegrationTests.Extensions;

internal static class TestDbContextFactoryExtensions
{
    internal static async Task AddPlayersToDatabase(this FplDashboardDbContext database, int[] playerIds) => await database.Players.AddRangeAsync(playerIds.Select(p => new Player() { Id = p }));
}