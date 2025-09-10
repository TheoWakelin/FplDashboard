using FplDashboard.DataModel.Models;
using FplDashboard.ETL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FplDashboard.ETL.IntegrationTests.Helpers;
using FplDashboard.ETL.IntegrationTests.TestData;
using Moq;

namespace FplDashboard.ETL.IntegrationTests;

public class FplSyncRunnerTestsPlayers : FplSyncRunnerTestBase
{
    [Fact]
    public async Task SyncAsync_AddsNewPlayer()
    {
        // Arrange
        var apiResponse = MockApiResponseHelper.CreateApiResponse(
            teams: [TeamData.ArsenalFromEtl],
            players: [PlayerData.SakaFromEtl]);
        ApiClient.Setup(c => c.GetMainFplData(It.IsAny<CancellationToken>())).ReturnsAsync(apiResponse);

        // Act
        await Sut.RunSyncAsync(CancellationToken.None);

        // Assert
        var dbPlayers = await Database.Players.ToListAsync();
        Assert.Single(dbPlayers);
        Assert.Contains(dbPlayers, p => p.WebName == PlayerData.SakaFromEtl.WebName);
    }
    
    [Fact]
    public async Task SyncAsync_UpdatesPlayersTeamFollowingTransfer()
    {
        // Arrange
        await Database.Players.AddAsync(PlayerData.Eze);
        await Database.SaveChangesAsync();
        var apiResponse = MockApiResponseHelper.CreateApiResponse(
            teams: [TeamData.ArsenalFromEtl, TeamData.CrystalPalaceFromEtl],
            players: [PlayerData.EzeTransferred]);
        ApiClient.Setup(c => c.GetMainFplData(It.IsAny<CancellationToken>())).ReturnsAsync(apiResponse);

        // Act
        await Sut.RunSyncAsync(CancellationToken.None);

        // Assert
        var dbPlayer = await Database.Players.FirstOrDefaultAsync(p => p.Id == PlayerData.EzeTransferred.Id);
        Assert.NotNull(dbPlayer);
        Assert.Equal(TeamData.Arsenal.Id, dbPlayer.TeamId);
        Assert.Equal(PlayerData.EzeTransferred.NowCost, dbPlayer.NowCost);
    }
}

