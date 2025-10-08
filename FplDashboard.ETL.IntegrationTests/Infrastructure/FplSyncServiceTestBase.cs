using FplDashboard.DataModel;
using FplDashboard.ETL.Interfaces;
using FplDashboard.ETL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace FplDashboard.ETL.IntegrationTests.Infrastructure;

public abstract class FplSyncRunnerTestBase : IDisposable
{
    protected readonly FplDashboardDbContext Database;
    protected readonly FplSyncRunner Sut;
    protected readonly Mock<IFplApiClient> ApiClient;

    protected FplSyncRunnerTestBase()
    {
        var dbName = $"TestDb_{Guid.NewGuid()}_Class";
        var options = new DbContextOptionsBuilder<FplDashboardDbContext>()
            .UseInMemoryDatabase(dbName)
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        Database = new FplDashboardDbContext(options);
        ApiClient = new Mock<IFplApiClient>();
        Sut = new FplSyncRunner(
            new Mock<ILogger<FplSyncRunner>>().Object,
            ApiClient.Object,
            Database,
            new TeamSyncService(Database),
            new GameWeekSyncService(Database),
            new PlayerSyncService(Database),
            new TeamGameWeekSyncService(Database),
            new PlayerGameWeekSyncService(Database),
            new PlayerNewsSyncService(Database),
            new FixtureSyncService(Database)
        );
        ClearDatabase();
    }

    private void ClearDatabase()
    {
        Database.Players.RemoveRange(Database.Players);
        Database.Teams.RemoveRange(Database.Teams);
        Database.GameWeeks.RemoveRange(Database.GameWeeks);
        Database.PlayerGameWeekData.RemoveRange(Database.PlayerGameWeekData);
        Database.TeamGameWeekData.RemoveRange(Database.TeamGameWeekData);
        Database.PlayerNews.RemoveRange(Database.PlayerNews);
        Database.Fixtures.RemoveRange(Database.Fixtures);
        Database.SaveChanges();
    }

    public void Dispose() => Database.Dispose();
}
