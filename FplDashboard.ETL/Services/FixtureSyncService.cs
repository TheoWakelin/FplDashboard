using FplDashboard.DataModel;
using FplDashboard.ETL.Models;
using Microsoft.EntityFrameworkCore;
using Fixture = FplDashboard.DataModel.Models.Fixture;

namespace FplDashboard.ETL.Services;

public class FixtureSyncService(FplDashboardDbContext database)
{
    public async Task SyncAsync(List<Fixture> fixtures, RelevantGameWeeks gameWeeks, CancellationToken cancellationToken)
    {
        // Check if last fixture for the season exists
        var lastFixtureExists = await database.Fixtures.AnyAsync(f => f.EventId == gameWeeks.FinalGameWeekId, cancellationToken);

        if (!lastFixtureExists)
        {
            // Initial sync: bulk insert all fixtures
            await database.Fixtures.AddRangeAsync(fixtures, cancellationToken);
            return;
        }

        // Update only current and previous game week fixtures
        var gameWeekIdsToUpdate = new List<int> { gameWeeks.CurrentGameWeekId, gameWeeks.PreviousGameWeekId };
        var fixturesToUpdate = fixtures.Where(f => gameWeekIdsToUpdate.Contains(f.EventId)).ToList();
        var existingFixtures = await database.Fixtures
            .Where(f => gameWeekIdsToUpdate.Contains(f.EventId))
            .ToListAsync(cancellationToken);

        foreach (var fixture in fixturesToUpdate)
        {
            var existing = existingFixtures.FirstOrDefault(f => f.EventId == fixture.EventId && f.TeamAId == fixture.TeamAId && f.TeamHId == fixture.TeamHId);
            existing?.CopyMutablePropertiesFrom(fixture);
        }
    }
}
