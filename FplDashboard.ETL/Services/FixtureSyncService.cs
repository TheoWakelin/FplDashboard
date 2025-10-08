using System.Text.Json;
using FplDashboard.DataModel;
using FplDashboard.ETL.Models;
using Microsoft.EntityFrameworkCore;
using Fixture = FplDashboard.DataModel.Models.Fixture;
using FixtureFromEtl = FplDashboard.ETL.Models.Fixture;

namespace FplDashboard.ETL.Services;

public class FixtureSyncService(FplDashboardDbContext database)
{
    public async Task MapFixturesFromApiAndAddToDatabase(string fixturesResponse, RelevantGameWeeks relevantGameWeeks, CancellationToken stoppingToken)
    {
        var fixturesEtl = JsonSerializer.Deserialize<List<FixtureFromEtl>>(fixturesResponse);
        if (fixturesEtl == null) return;
        
        // Build lookup from GameWeekNumber to Id.
        var gameWeekLookup = await database.GameWeeks.ToDictionaryAsync(gw => gw.GameWeekNumber, gw => gw.Id, cancellationToken: stoppingToken);
        
        var fixtures = fixturesEtl
            .Where(f => gameWeekLookup.ContainsKey(f.GameweekId))
            .Select(f => f.ToDataModelFixture(gameWeekLookup[f.GameweekId]))
            .ToList();
        
        await SyncAsync(fixtures, relevantGameWeeks, stoppingToken);
        await database.SaveChangesAsync(stoppingToken);
    }
    
    public async Task SyncAsync(List<Fixture> fixtures, RelevantGameWeeks gameWeeks, CancellationToken cancellationToken)
    {
        // Check if last fixture for the season exists
        var lastFixtureExists = await database.Fixtures.AnyAsync(f => f.GameweekId == gameWeeks.FinalGameWeekId, cancellationToken);

        if (!lastFixtureExists)
        {
            // Initial sync: bulk insert all fixtures
            await database.Fixtures.AddRangeAsync(fixtures, cancellationToken);
            return;
        }

        // Update only current and previous game week fixtures
        var gameWeekIdsToUpdate = new List<int> { gameWeeks.CurrentGameWeekId, gameWeeks.PreviousGameWeekId };
        var fixturesToUpdate = fixtures.Where(f => gameWeekIdsToUpdate.Contains(f.GameweekId)).ToList();
        var existingFixtures = await database.Fixtures
            .Where(f => gameWeekIdsToUpdate.Contains(f.GameweekId))
            .ToListAsync(cancellationToken);

        foreach (var fixture in fixturesToUpdate)
        {
            var existing = existingFixtures.FirstOrDefault(f => f.GameweekId == fixture.GameweekId && f.AwayTeamId == fixture.AwayTeamId && f.HomeTeamId == fixture.HomeTeamId);
            existing?.CopyMutablePropertiesFrom(fixture);
        }
    }
}
