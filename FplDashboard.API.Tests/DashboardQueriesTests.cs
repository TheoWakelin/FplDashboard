using System.Data.Common;
using FplDashboard.API.Features.Dashboard;
using FplDashboard.API.Features.Shared;
using FplDashboard.DataModel;
using FplDashboard.DataModel.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FplDashboard.API.Tests;

public class DashboardQueriesTests : IDisposable
{
    private readonly DbConnection _connection;
    private readonly FplDashboardDbContext _dbContext;
    private readonly DashboardQueries _dashboardQueries;

    public DashboardQueriesTests()
    {
        // Setup in-memory SQLite connection
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<FplDashboardDbContext>()
            .UseSqlite(_connection)
            .Options;
        _dbContext = new FplDashboardDbContext(options);
        _dbContext.Database.EnsureCreated();

        // Use the connection factory for DashboardQueries
        var connectionFactory = new TestSqliteConnectionFactory((SqliteConnection)_connection);
        _dashboardQueries = new DashboardQueries(connectionFactory, new Mock<IGeneralQueries>().Object);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task GetDashboardData_ReturnsExpectedResults_WhenDataIsSeeded()
    {
        // Seed minimal valid data
        var team = new Team { Id = 1, Name = "Team A" };
        var player = new Player { Id = 1, WebName = "Player 1", TeamId = 1, PointsPerGame = 10 };
        var gameweek = new GameWeek { Id = 1, GameWeekNumber = 1, YearSeasonStarted = 2025 };
        var playerNews = new PlayerNews { PlayerId = 1, NewsAdded = DateTime.UtcNow, News = "Injured" };
        var teamGameWeekData = new TeamGameWeekData { TeamId = 1, GameWeekId = 1, StrengthAttackHome = 80, StrengthDefenceHome = 70, StrengthAttackAway = 75, StrengthDefenceAway = 65 };
        var fixture = new Fixture { Id = 1, GameweekId = 1, GameWeek = gameweek, AwayTeamId = 1, HomeTeamId = 1, Finished = false };
        await _dbContext.Teams.AddAsync(team);
        await _dbContext.Players.AddAsync(player);
        await _dbContext.GameWeeks.AddAsync(gameweek);
        await _dbContext.PlayerNews.AddAsync(playerNews);
        await _dbContext.TeamGameWeekData.AddAsync(teamGameWeekData);
        await _dbContext.AddAsync(fixture);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _dashboardQueries.GetDashboardDataAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        // Further asserts can be added for each returned property
    }
}