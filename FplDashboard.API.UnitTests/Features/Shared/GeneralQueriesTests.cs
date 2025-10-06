using Dapper;
using Moq;
using Moq.Dapper;
using FplDashboard.API.Features.Shared;
using FplDashboard.API.UnitTests.Infrastructure;

namespace FplDashboard.API.UnitTests.Features.Shared
{
    public class GeneralQueriesTests : QueriesTestBase
    {
        private readonly GeneralQueries _sut;
        private const int GameWeekId = 5;

        public GeneralQueriesTests()
        {
            _sut = new GeneralQueries(MockConnectionFactory.Object, MockCacheService.Object);
        }

        [Fact]
        public async Task GetCurrentGameWeekIdAsync_ReturnsCachedValue_WhenCacheHit()
        {
            // Arrange
            MockCacheService.Setup(m => m.Get<int?>(CacheKeys.CurrentGameWeekId)).Returns(GameWeekId);

            // Act
            var result = await _sut.GetCurrentGameWeekIdAsync(CancellationToken.None);

            // Assert
            Assert.Equal(GameWeekId, result);
            MockConnectionFactory.Verify(m => m.CreateConnection(), Times.Never);
        }

        [Fact]
        public async Task GetCurrentGameWeekIdAsync_QueriesDatabaseAndCaches_WhenCacheMiss()
        {
            // Arrange
            MockCacheService.Setup(m => m.Get<int?>(CacheKeys.CurrentGameWeekId)).Returns((int?)null);
            MockDbConnection.SetupDapperAsync(c => c.QuerySingleAsync<int>(It.IsAny<CommandDefinition>())).ReturnsAsync(GameWeekId);

            // Act
            var result = await _sut.GetCurrentGameWeekIdAsync(CancellationToken.None);

            // Assert
            Assert.Equal(GameWeekId, result);
            MockConnectionFactory.Verify(m => m.CreateConnection(), Times.Once);
            MockCacheService.Verify(m => m.Set(CacheKeys.CurrentGameWeekId, It.IsAny<int>()), Times.Once);
        }
    }
}
