using FplDashboard.DataModel.Models;
using FplDashboard.ETL.IntegrationTests.Extensions;
using FplDashboard.ETL.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using FplDashboard.ETL.IntegrationTests.Infrastructure;
using FplDashboard.ETL.Models;
using Moq;

namespace FplDashboard.ETL.IntegrationTests;

public class FplSyncRunnerTestsPlayerNews : FplSyncRunnerTestBase
{
    [Fact]
    public async Task SyncAsync_AddsNewAndIgnoresOlderNews()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var currentLatestNews = new PlayerNews { PlayerId = 1, NewsAdded = now.AddDays(-2), News = "Old injury" };
        await Database.AddPlayersToDatabase([1,2]);
        await Database.PlayerNews.AddAsync(currentLatestNews);
        await Database.SaveChangesAsync();

        var players = new List<PlayerElement>
        {
            new() { Id = 1, NewsAdded = now.AddDays(-1), News = "Fit to play" },
            new() { Id = 2, NewsAdded = now, News = "Player suspended" },
            new() { Id = 3, NewsAdded = now.AddDays(-3), News = "Player transferred" },
        };
        
        ApiClient.Setup(c => c.GetMainFplData(It.IsAny<CancellationToken>()))
            .ReturnsAsync(MockApiResponseHelper.CreateApiResponse(players: players));
        
        // Act
        await Sut.RunSyncAsync(CancellationToken.None);
        
        // Assert
        var dbNews = await Database.PlayerNews.ToListAsync();
        
        // Here we assert that the new news for player 1 and 2 are added and the older news is not.
        Assert.Equal(3, dbNews.Count);
        Assert.DoesNotContain("Player transferred", dbNews[0].News);
    }
}

