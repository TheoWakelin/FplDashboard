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
    }
}

