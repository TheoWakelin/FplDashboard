using FplDashboard.DataModel.Models;
using FplDashboard.ETL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FplDashboard.ETL.IntegrationTests.Helpers;
using FplDashboard.ETL.IntegrationTests.TestData;
using Moq;

namespace FplDashboard.ETL.IntegrationTests;

public class FplSyncRunnerTestsPlayerGameWeeks : FplSyncRunnerTestBase
{
    [Fact]
    public async Task SyncAsync_AddsNewPlayerGameWeekData()
    {
        // Arrange
        var apiResponse = MockApiResponseHelper.CreateApiResponse(
            teams: [TeamData.CrystalPalaceFromEtl],
            players: [PlayerData.EzeTransferred]);
        ApiClient.Setup(x => x.GetMainFplData(It.IsAny<CancellationToken>())).ReturnsAsync(apiResponse);

        // Act
        await Sut.RunSyncAsync(CancellationToken.None);

        // Assert
        var dbPlayerGameWeekData = await Database.PlayerGameWeekData.FirstOrDefaultAsync(p => p.PlayerId == 2);
        Assert.NotNull(dbPlayerGameWeekData);
        Assert.Equal(75, dbPlayerGameWeekData.NowCost);
        Assert.Equal(15, dbPlayerGameWeekData.EventPoints);
    }

    [Fact]
    public async Task SyncAsync_UpdatesMutableProperties()
    {
        // Arrange
        await Database.Players.AddAsync(PlayerData.Saka);
        await Database.GameWeeks.AddAsync(EventData.CurrentGameWeek);

        await Database.PlayerGameWeekData.AddAsync(new PlayerGameWeekData
        {
            PlayerId = 1,
            GameWeekId = EventData.CurrentGameWeek.Id,
            EventPoints = 0,
            NowCost = 100
        });
        await Database.SaveChangesAsync();

        var apiResponse = MockApiResponseHelper.CreateApiResponse(
            teams: [TeamData.ArsenalFromEtl],
            players: [PlayerData.SakaFromEtl]
        );
        ApiClient.Setup(x => x.GetMainFplData(It.IsAny<CancellationToken>())).ReturnsAsync(apiResponse);

        // Act
        await Sut.RunSyncAsync(CancellationToken.None);

        // Assert
        var dbPlayerGameWeekData = await Database.PlayerGameWeekData.FirstOrDefaultAsync();
        Assert.NotNull(dbPlayerGameWeekData);
        Assert.Equal(1, dbPlayerGameWeekData.PlayerId);
        Assert.Equal(110, dbPlayerGameWeekData.NowCost);
        Assert.Equal(15, dbPlayerGameWeekData.EventPoints);
    }
}
