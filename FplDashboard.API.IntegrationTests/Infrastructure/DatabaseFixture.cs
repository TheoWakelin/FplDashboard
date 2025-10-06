using AutoFixture;
using FplDashboard.API.IntegrationTests.Infrastructure.Models;
using FplDashboard.DataModel;
using FplDashboard.DataModel.Models;
using Microsoft.EntityFrameworkCore;
using Fixture = AutoFixture.Fixture;
using DomainFixture = FplDashboard.DataModel.Models.Fixture;

namespace FplDashboard.API.IntegrationTests.Infrastructure;

public class DatabaseFixture : IAsyncLifetime
{
    public FplDashboardDbContext DbContext { get; private set; } = null!;
    public string ConnectionString { get; private set; } = string.Empty;
    public DashboardSeededData SeededData { get; private set; } = null!;
    
    private Fixture Fixture { get; } = new();
    private DatabaseSeeder Seeder => new(DbContext, Fixture);

    public async Task InitializeAsync()
    {
        ConnectionString = TestConfiguration.Database.GetTestConnectionString("Integration");

        var options = new DbContextOptionsBuilder<FplDashboardDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;
        DbContext = new FplDashboardDbContext(options);
        await DbContext.Database.EnsureCreatedAsync();

        SeededData = await Seeder.SeedAllTestDataAsync();
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
