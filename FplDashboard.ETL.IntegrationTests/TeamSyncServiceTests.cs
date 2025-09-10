using FplDashboard.ETL.IntegrationTests.Helpers;
using FplDashboard.ETL.IntegrationTests.Infrastructure;
using FplDashboard.ETL.IntegrationTests.TestData;
using Microsoft.EntityFrameworkCore;
using Moq;
using TeamFromApi = FplDashboard.ETL.Models.Team;

namespace FplDashboard.ETL.IntegrationTests;

public class TeamSyncServiceTests : FplSyncRunnerTestBase
{
    [Fact]
    public async Task SyncAsync_AddsNewTeams()
    {
        // Arrange
        var teams = new List<TeamFromApi> { TeamData.ArsenalFromEtl, TeamData.CrystalPalaceFromEtl };
        
        ApiClient.Setup(c => c.GetMainFplData(It.IsAny<CancellationToken>()))
            .ReturnsAsync(MockApiResponseHelper.CreateApiResponse(teams: teams));
        
        // Act
        await Sut.RunSyncAsync(CancellationToken.None);
        
        // Assert
        var dbTeams = await Database.Teams.ToListAsync();
        Assert.Equal(2, dbTeams.Count);
        Assert.Contains(dbTeams, t => t.Name == TeamData.ArsenalFromEtl.Name);
        Assert.Contains(dbTeams, t => t.Name == TeamData.CrystalPalaceFromEtl.Name);
    }

    [Fact]
    public async Task SyncAsync_DoesNotUpdateImmutableProperties()
    {
        // Arrange
        await Database.Teams.AddAsync(TeamData.Arsenal);
        await Database.SaveChangesAsync();

        // Attempt to change the name and shortname properties which are immutable.
        var updatedTeam = new TeamFromApi { Id = 1, Name = "Coventry", ShortName = "Cov" };
        
        ApiClient.Setup(c => c.GetMainFplData(It.IsAny<CancellationToken>()))
            .ReturnsAsync(MockApiResponseHelper.CreateApiResponse(teams: [updatedTeam]));
        
        // Act
        await Sut.RunSyncAsync(CancellationToken.None);
        
        // Assert
        var dbTeam = await Database.Teams.FirstAsync(t => t.Id == 1);

        // Name should remain unchanged
        Assert.Equal(TeamData.Arsenal.Name, dbTeam.Name); 
    }
}
