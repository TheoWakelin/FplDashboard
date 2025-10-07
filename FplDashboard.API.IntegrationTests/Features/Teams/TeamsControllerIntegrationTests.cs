using FplDashboard.API.Features.Teams.Models;
using FplDashboard.API.IntegrationTests.Infrastructure;

namespace FplDashboard.API.IntegrationTests.Features.Teams;

public class TeamsControllerIntegrationTests(DatabaseFixture fixture) : BaseIntegrationTest(fixture)
{
    private const string TeamsFixturesEndpoint = "/teams/fixtures";

    [Fact]
    public async Task GetTeamFixtures_WithSeededData_ReturnsCorrectTeamFixtures()
    {
        // Act
        var result = await ApiClient.GetAndExpectSuccessAsync<List<TeamFixturesDto>>(TeamsFixturesEndpoint);

        // Assert
        ValidateTeamFixtures(result);
    }

    private static void ValidateTeamFixtures(List<TeamFixturesDto> result)
    {
        Assert.NotEmpty(result);
        foreach (var team in result)
        {
            Assert.False(string.IsNullOrEmpty(team.TeamName));
            Assert.NotEmpty(team.Fixtures);
            
            Assert.Equal(team.Fixtures.Sum(f => f.AttackingStrength), team.TotalAttackingStrength);
            Assert.Equal(team.Fixtures.Sum(f => f.DefensiveStrength), team.TotalDefensiveStrength);
            Assert.Equal(team.TotalAttackingStrength + team.TotalDefensiveStrength, team.CumulativeStrength);
        }
        
        for (var i = 0; i < result.Count - 1; i++)
        {
            Assert.True(result[i].CumulativeStrength >= result[i + 1].CumulativeStrength, "Teams should be ordered by cumulative strength descending");
        }
    }
}

