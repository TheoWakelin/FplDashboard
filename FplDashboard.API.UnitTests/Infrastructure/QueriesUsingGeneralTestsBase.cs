using FplDashboard.API.Features.Shared;
using Moq;

namespace FplDashboard.API.UnitTests.Infrastructure
{
    public abstract class QueriesUsingGeneralTestsBase : QueriesTestBase
    {
        protected readonly Mock<IGeneralQueries> MockGeneralQueries;
        private const int CurrentGameWeekId = 2025;

        protected QueriesUsingGeneralTestsBase()
        {
            MockGeneralQueries = new Mock<IGeneralQueries>();
            MockGeneralQueries.Setup(m => m.GetCurrentGameWeekIdAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(CurrentGameWeekId);
        }
    }
}
