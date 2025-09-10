using FplDashboard.DataModel.Models;
using FplDashboard.ETL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FplDashboard.ETL.IntegrationTests.Helpers;
using FplDashboard.ETL.IntegrationTests.TestData;
using Moq;
using TeamFromEtl = FplDashboard.ETL.Models.Team;

namespace FplDashboard.ETL.IntegrationTests;

public class FplSyncRunnerTestsTeamGameWeeks : FplSyncRunnerTestBase
{
    [Fact]
    public async Task SyncAsync_AddsNewTeamGameWeekData()
    {
        // Arrange
        var apiResponse = MockApiResponseHelper.CreateApiResponse(
            teams: [new TeamFromEtl { Id = 1, Name = "Arsenal", ShortName = "ARS", Points = 21 }]);
        ApiClient.Setup(x => x.GetMainFplData(It.IsAny<CancellationToken>())).ReturnsAsync(apiResponse);

        // Act
        await Sut.RunSyncAsync(CancellationToken.None);

        // Assert
        var dbTeamGameWeekData = await Database.TeamGameWeekData.FirstOrDefaultAsync();
        Assert.NotNull(dbTeamGameWeekData);
        Assert.Equal(1, dbTeamGameWeekData.TeamId);
        Assert.Equal(EventData.CurrentGameWeek.Id, dbTeamGameWeekData.GameWeekId);
        Assert.Equal(21, dbTeamGameWeekData.Points);
    }

    [Fact]
    public async Task SyncAsync_UpdatesMutableProperties()
    {
        // Arrange
        await Database.Teams.AddAsync(TeamData.CrystalPalace);
        await Database.GameWeeks.AddAsync(EventData.CurrentGameWeek);
        await Database.TeamGameWeekData.AddAsync(new TeamGameWeekData {
            TeamId = 2,
            GameWeekId = EventData.CurrentGameWeek.Id,
            Points = 13
        });
        await Database.SaveChangesAsync();
        var apiResponse = MockApiResponseHelper.CreateApiResponse(
            teams: [new TeamFromEtl { Id = 2, Name = "Crystal Palace", ShortName = "CRY", Points = 17 }]
        );
        ApiClient.Setup(x => x.GetMainFplData(It.IsAny<CancellationToken>())).ReturnsAsync(apiResponse);
    
        // Act
        await Sut.RunSyncAsync(CancellationToken.None);
    
        // Assert
        var dbTeamGameWeekData = await Database.TeamGameWeekData.FirstOrDefaultAsync();
        Assert.NotNull(dbTeamGameWeekData);
        Assert.Equal(2, dbTeamGameWeekData.TeamId);
        Assert.Equal(17, dbTeamGameWeekData.Points);
    }
}
