using FplDashboard.ETL.IntegrationTests.Infrastructure;
using FplDashboard.ETL.IntegrationTests.Helpers;
using FplDashboard.ETL.IntegrationTests.TestData;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FplDashboard.ETL.IntegrationTests;

public class FplSyncRunnerTestsFixtures : FplSyncRunnerTestBase
{
    [Fact]
    public async Task InitialSync_AddsAllFixtures()
    {
        // Arrange
        ApiClient.Setup(x => x.GetFixturesData(It.IsAny<CancellationToken>()))
            .ReturnsAsync(MockApiResponseHelper.CreateFixtureApiResponse(FixtureData.InitialGameweek4FromEtl));
        ApiClient.Setup(x => x.GetMainFplData(It.IsAny<CancellationToken>()))
            .ReturnsAsync(MockApiResponseHelper.CreateApiResponse());
        // Act
        await Sut.RunSyncAsync(CancellationToken.None);
        
        // Assert
        var storedFixtures = await Database.Fixtures.ToListAsync();
        Assert.Single(storedFixtures);
        Assert.False(storedFixtures.Single().Finished);
    }

    [Fact]
    public async Task UpdateSync_UpdatesCurrentAndPreviousGameweekFixturesOnly()
    {
        // Arrange
        await Database.Teams.AddRangeAsync([TeamData.Arsenal, TeamData.CrystalPalace]);
        await Database.GameWeeks.AddRangeAsync([EventData.CurrentGameWeek]);
        await Database.Fixtures.AddRangeAsync([FixtureData.InitialGameweek4[0], FixtureData.Gameweek38[0]]);
        await Database.SaveChangesAsync();
        var updateFixtures = FixtureData.UpdatedGameweek4FromEtl.Concat(FixtureData.Gameweek38FromEtl).ToList();
        ApiClient.Setup(x => x.GetFixturesData(It.IsAny<CancellationToken>()))
            .ReturnsAsync(MockApiResponseHelper.CreateFixtureApiResponse(updateFixtures));
        ApiClient.Setup(x => x.GetMainFplData(It.IsAny<CancellationToken>())).ReturnsAsync(MockApiResponseHelper.CreateApiResponse());
        
        // Act
        await Sut.RunSyncAsync(CancellationToken.None);
        
        // Assert
        var fixtures = await Database.Fixtures.ToListAsync();
        Assert.Equal(2, fixtures.Count);
        var gw4Fixture = fixtures.First(f => f.EventId == EventData.CurrentGameWeek.Id);
        Assert.True(gw4Fixture.Finished);
        Assert.Equal(2, gw4Fixture.TeamAScore);
        Assert.Equal(1, gw4Fixture.TeamHScore); 
    }
}
