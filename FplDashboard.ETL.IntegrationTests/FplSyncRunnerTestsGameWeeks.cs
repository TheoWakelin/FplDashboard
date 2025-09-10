using FplDashboard.DataModel.Models;
using FplDashboard.ETL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using FplDashboard.ETL.IntegrationTests.Helpers;
using FplDashboard.ETL.IntegrationTests.TestData;

namespace FplDashboard.ETL.IntegrationTests;

public class FplSyncRunnerTestsGameWeeks : FplSyncRunnerTestBase
{
    [Fact]
    public async Task SyncAsync_AddsNewGameWeek()
    {
        // Arrange
        ApiClient.Setup(c => c.GetMainFplData(It.IsAny<CancellationToken>()))
            .ReturnsAsync(MockApiResponseHelper.CreateApiResponse([EventData.CurrentGameWeekFromEtl]));

        // Act
        await Sut.RunSyncAsync(CancellationToken.None);

        // Assert
        var dbGameWeek = await Database.GameWeeks.FirstOrDefaultAsync(gw => gw.GameWeekNumber == 4);
        Assert.NotNull(dbGameWeek);
        Assert.Equal(GameWeekStatus.Current, dbGameWeek.Status);
    }

    [Fact]
    public async Task SyncAsync_UpdatesMutableProperties()
    {
        // Arrange
        await Database.GameWeeks.AddAsync(new GameWeek
        {
            GameWeekNumber = 3,
            Status = GameWeekStatus.Current,
            AverageEntryScore = null,
            HighestScore = null
        });
        await Database.SaveChangesAsync();
        ApiClient.Setup(c => c.GetMainFplData(It.IsAny<CancellationToken>()))
            .ReturnsAsync(MockApiResponseHelper.CreateApiResponse([EventData.PreviousGameWeek, EventData.CurrentGameWeekFromEtl]));
        
        // Act
        await Sut.RunSyncAsync(CancellationToken.None);
        
        // Assert
        var dbGameWeek = await Database.GameWeeks.ToArrayAsync();
        Assert.Equal(2, dbGameWeek.Length);

        var updatedGameWeek = dbGameWeek.First(gw => gw.GameWeekNumber == 3);
        Assert.Equal(GameWeekStatus.Previous, updatedGameWeek.Status);
        Assert.Equal(55, updatedGameWeek.AverageEntryScore);
        Assert.Equal(110, updatedGameWeek.HighestScore);
    }
}
