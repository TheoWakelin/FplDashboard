using Dapper;
using FplDashboard.API.Features.Dashboard;
using FplDashboard.API.Features.Dashboard.Models;
using FplDashboard.API.Features.Shared;
using FplDashboard.API.UnitTests.Infrastructure;
using Moq;
using Moq.Dapper;

namespace FplDashboard.API.UnitTests.Features.Dashboard
{
    public class DashboardQueriesTests : QueriesUsingGeneralTestsBase
    {
        private readonly DashboardQueries _sut;
        private readonly DashboardDataDto _cachedObject;

        public DashboardQueriesTests()
        {
            _sut = new DashboardQueries(MockConnectionFactory.Object, MockGeneralQueries.Object, MockCacheService.Object);
            _cachedObject = new DashboardDataDto {
                PlayerNews = [
                    new PlayerNewsDto {
                        NewsAdded = DateTime.UtcNow,
                        PlayerName = "Test Player",
                        TeamName = "Test Team",
                        News = "Test News"
                    }
                ],
                TopTeams = [
                    new TeamStrengthDto("TeamA", 10, TeamStrengthCategory.Top)
                ],
                BottomTeams = [
                    new TeamStrengthDto("TeamB", 5, TeamStrengthCategory.Bottom)
                ]
            };
        }

        [Fact]
        public async Task GetDashboardDataAsync_CachesResultAfterFirstFetch()
        {
            // Arrange
            MockCacheService.Setup(m => m.Get<DashboardDataDto>(CacheKeys.DashboardData)).Returns((DashboardDataDto?)null);
            
            MockDbConnection.SetupDapperAsync(c => c.QueryAsync<PlayerNewsDto>(It.IsAny<CommandDefinition>()))
                .ReturnsAsync(_cachedObject.PlayerNews);

            MockDbConnection.SetupDapperAsync(c => c.QueryAsync<TeamStrengthDto>(It.IsAny<CommandDefinition>()))
                .ReturnsAsync(_cachedObject.TopTeams.Concat(_cachedObject.BottomTeams));
                
            MockGeneralQueries.Setup(m => m.GetCurrentGameWeekIdAsync(It.IsAny<CancellationToken>())).ReturnsAsync(2025);

            // Act
            var result = await _sut.GetDashboardDataAsync(CancellationToken.None);

            // Assert
            VerifyCacheSet<DashboardDataDto>(CacheKeys.DashboardData);
            Assert.Equivalent(_cachedObject.TopTeams, result.TopTeams);
            Assert.Equivalent(_cachedObject.BottomTeams, result.BottomTeams);
        }

        [Fact]
        public async Task GetDashboardDataAsync_ReturnsCachedDataIfPresent()
        {
            // Arrange
            MockCacheService.Setup(m => m.Get<DashboardDataDto>(CacheKeys.DashboardData)).Returns(_cachedObject);
            MockGeneralQueries.Setup(m => m.GetCurrentGameWeekIdAsync(It.IsAny<CancellationToken>())).ReturnsAsync(2025);

            // Act
            var result = await _sut.GetDashboardDataAsync(CancellationToken.None);

            // Assert
            Assert.Equal(_cachedObject, result);
            VerifyCacheNotSet<DashboardDataDto>(CacheKeys.DashboardData);
        }
    }
}
