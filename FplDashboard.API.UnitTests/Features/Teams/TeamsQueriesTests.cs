using Dapper;
using Moq;
using Moq.Dapper;
using FplDashboard.API.Features.Teams;
using FplDashboard.API.Features.Shared;
using FplDashboard.API.Features.Teams.Models;
using FplDashboard.API.UnitTests.Infrastructure;

namespace FplDashboard.API.UnitTests.Features.Teams
{
    public class TeamsQueriesTests : BaseUnitTest
    {
        private readonly TeamsQueries _sut;

        public TeamsQueriesTests()
        {
            var mockGeneralQueries = new Mock<IGeneralQueries>();
            _sut = new TeamsQueries(MockConnectionFactory.Object, mockGeneralQueries.Object, MockCacheService.Object);
        }

        [Fact]
        public async Task GetTeamFixturesAsync_ReturnsCachedValue_WhenCacheHit()
        {
            // Arrange
            var cachedFixtures = new List<TeamFixturesDto> { new() { TeamName = "TeamA", Fixtures = [] } };
            MockCacheService.Setup(m => m.Get<List<TeamFixturesDto>>(CacheKeys.TeamFixtures)).Returns(cachedFixtures);

            // Act
            var result = await _sut.GetTeamFixturesAsync(CancellationToken.None);

            // Assert
            Assert.Equal(cachedFixtures, result);
            MockConnectionFactory.Verify(m => m.CreateConnection(), Times.Never);
        }

        [Fact]
        public async Task GetTeamFixturesAsync_QueriesDatabaseAndCaches_WhenCacheMiss()
        {
            // Arrange
            MockCacheService.Setup(m => m.Get<List<TeamFixturesDto>>(CacheKeys.TeamFixtures)).Returns((List<TeamFixturesDto>?)null);
            MockDbConnection.SetupDapperAsync(c => c.QueryAsync<FixtureScoreDto>(It.IsAny<CommandDefinition>()))
                .ReturnsAsync(new List<FixtureScoreDto> { new() { TeamName = "TeamA" }, new() { TeamName = "TeamB" } });

            // Act
            await _sut.GetTeamFixturesAsync(CancellationToken.None);

            // Assert
            MockConnectionFactory.Verify(m => m.CreateConnection(), Times.Once);
            MockCacheService.Verify(m => m.Set(CacheKeys.TeamFixtures, It.IsAny<List<TeamFixturesDto>>()), Times.Once);
        }
    }
}
