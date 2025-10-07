using System.Data;
using FplDashboard.API.Features.Shared;
using FplDashboard.API.Infrastructure;
using Moq;

namespace FplDashboard.API.UnitTests.Infrastructure
{
    public abstract class QueriesTestBase
    {
        protected readonly Mock<IDbConnectionFactory> MockConnectionFactory;
        protected readonly Mock<ICacheService> MockCacheService;
        protected readonly Mock<IDbConnection> MockDbConnection;

        protected QueriesTestBase()
        {
            MockConnectionFactory = new Mock<IDbConnectionFactory>();
            MockCacheService = new Mock<ICacheService>();
            MockDbConnection = new Mock<IDbConnection>();
            MockConnectionFactory.Setup(m => m.CreateConnection()).Returns(MockDbConnection.Object);
        }

        protected void VerifyCacheSet<T>(string key) => MockCacheService.Verify(m => m.Set(key, It.IsAny<T>()), Times.Once());

        protected void VerifyCacheNotSet<T>(string key) => MockCacheService.Verify(m => m.Set(key, It.IsAny<T>()), Times.Never());

        protected void VerifyConnectionCreated() => MockConnectionFactory.Verify(m => m.CreateConnection(), Times.Once());

        protected void VerifyConnectionNotCreated() => MockConnectionFactory.Verify(m => m.CreateConnection(), Times.Never());
    }
}