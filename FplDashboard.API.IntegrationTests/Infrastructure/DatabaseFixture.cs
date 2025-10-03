using AutoFixture;
using FplDashboard.DataModel;
using FplDashboard.DataModel.Models;
using Microsoft.EntityFrameworkCore;
using Fixture = AutoFixture.Fixture;

namespace FplDashboard.API.Tests.Infrastructure;

public class DatabaseFixture : IAsyncLifetime
{
    public FplDashboardDbContext DbContext { get; private set; } = null!;
    public string ConnectionString { get; private set; } = string.Empty;
    public List<Team> SeededTeams { get; private set; } = [];
    private List<Player> SeededPlayers { get; set; } = [];
    private GameWeek SeededGameWeek { get; set; } = null!;
    
    private Fixture Fixture { get; } = new();

    public async Task InitializeAsync()
    {
        ConnectionString = TestConfiguration.Database.GetTestConnectionString("Integration");

        var options = new DbContextOptionsBuilder<FplDashboardDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;
        DbContext = new FplDashboardDbContext(options);
        await DbContext.Database.EnsureCreatedAsync();

        await SeedMinimalDataAsync();
    }

    private async Task SeedMinimalDataAsync()
    {
        // Teams
        SeededTeams = Fixture.GetTeamsFromNames(TestConfiguration.TestData.DefaultTeamNames);
        await DbContext.Teams.AddRangeAsync(SeededTeams);
        await DbContext.SaveChangesAsync();

        // GameWeek
        SeededGameWeek = Fixture.Build<GameWeek>()
            .With(gw => gw.GameWeekNumber, 1)
            .With(gw => gw.YearSeasonStarted, 2025)
            .With(gw => gw.Status, GameWeekStatus.Current)
            .OmitAutoProperties()
            .Create();
        await DbContext.GameWeeks.AddAsync(SeededGameWeek);
        await DbContext.SaveChangesAsync();

        // Players
        SeededPlayers = Fixture.GetPlayersFromSeededPlayers(TestConfiguration.TestData.SeededPlayers, SeededTeams[0].Id);
        await DbContext.Players.AddRangeAsync(SeededPlayers);
        await DbContext.SaveChangesAsync();

        // PlayerGameWeekData
        var playerGameWeekData = SeededPlayers.Select(player => Fixture.Build<PlayerGameWeekData>()
            .With(pg => pg.PlayerId, player.Id)
            .With(pg => pg.GameWeekId, SeededGameWeek.Id)
            .Without(pg => pg.GameWeek)
            .Without(pg => pg.Player)
            .Create());
        
        await DbContext.PlayerGameWeekData.AddRangeAsync(playerGameWeekData);
        await DbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        try
        {
            await DbContext.Database.EnsureDeletedAsync();
            Console.WriteLine($"Test database deleted: {ConnectionString}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Failed to delete test database: {ex.Message}");
        }
        finally
        {
            await DbContext.DisposeAsync();
        }
    }
}
