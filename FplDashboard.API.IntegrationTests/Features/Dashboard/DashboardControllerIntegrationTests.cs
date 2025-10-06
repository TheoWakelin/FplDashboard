using FplDashboard.API.Features.Dashboard.Models;
using FplDashboard.API.IntegrationTests.Infrastructure;

namespace FplDashboard.API.IntegrationTests.Features.Dashboard;

public class DashboardControllerIntegrationTests(DatabaseFixture fixture) : BaseIntegrationTest(fixture)
{
    private const string DashboardDataEndpoint = "/dashboard/dashboardData";

    [Fact]
    public async Task GetDashboardData_WithSeededData_ReturnsCorrectDashboardTiles()
    {
        // Act
        var result = await ApiClient.GetAndExpectSuccessAsync<DashboardDataDto>(DashboardDataEndpoint);

        // Assert
        ValidatePlayerNews(result);
        ValidateTopTeams(result);
        ValidateBottomTeams(result);
    }

    private static void ValidatePlayerNews(DashboardDataDto result)
    {
        Assert.NotEmpty(result.PlayerNews);
        
        var firstNews = result.PlayerNews.First();
        Assert.False(string.IsNullOrEmpty(firstNews.PlayerName));
        Assert.False(string.IsNullOrEmpty(firstNews.TeamName));
        Assert.False(string.IsNullOrEmpty(firstNews.News));
        Assert.True(firstNews.NewsAdded > DateTime.MinValue);

        // Verify news is ordered by NewsAdded descending
        var newsItems = result.PlayerNews.ToList();
        for (var i = 0; i < newsItems.Count - 1; i++)
        {
            Assert.True(newsItems[i].NewsAdded >= newsItems[i + 1].NewsAdded, "Player news should be ordered by NewsAdded descending");
        }
    }
    
    private static void ValidateTopTeams(DashboardDataDto result)
    {
        Assert.NotEmpty(result.TopTeams);
        Assert.All(result.TopTeams, team => Assert.Equal(TeamStrengthCategory.Top, team.Category));
        
        // Verify top teams are ordered by strength descending
        var topTeams = result.TopTeams.ToList();
        for (var i = 0; i < topTeams.Count - 1; i++)
        {
            Assert.True(topTeams[i].CumulativeStrength >= topTeams[i + 1].CumulativeStrength, "Top teams should be ordered by cumulative strength descending");
        }
        
        // Team Alpha should be in top teams (strongest ratings: 5,5,4,4)
        Assert.Equal("Team Alpha", result.TopTeams.First().TeamName);
    }
    
    private static void ValidateBottomTeams(DashboardDataDto result)
    {
        Assert.NotEmpty(result.BottomTeams);
        Assert.All(result.BottomTeams, team => Assert.Equal(TeamStrengthCategory.Bottom, team.Category));
        
        // Verify bottom teams are ordered by strength ascending
        var bottomTeams = result.BottomTeams.ToList();
        for (var i = 0; i < bottomTeams.Count - 1; i++)
        {
            Assert.True(bottomTeams[i].CumulativeStrength <= bottomTeams[i + 1].CumulativeStrength, "Bottom teams should be ordered by cumulative strength ascending");
        }
        
        // Team Beta should be in bottom teams (weakest ratings: 1,1,2,2)
        Assert.Equal("Team Beta", result.BottomTeams.First().TeamName);
    }
}
