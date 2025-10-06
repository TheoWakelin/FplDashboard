using FplDashboard.API.IntegrationTests.Infrastructure.Models;
using FplDashboard.DataModel.Models;

namespace FplDashboard.API.IntegrationTests.Infrastructure;

public static class TestConfiguration
{
    public static class Database
    {
        public static string GetTestConnectionString(string testName) =>
            $"Server=(localdb)\\mssqllocaldb;Database=FplDashboardTest_{testName}_{Guid.NewGuid()};Trusted_Connection=true;MultipleActiveResultSets=true";
    }
    
    public static class TestData
    {
        public static int DefaultPlayerCount => SeededPlayers.Length;
        public static int DefaultTeamCount => DefaultTeamNames.Length;
        public static string[] DefaultTeamNames => [
            "Team Alpha",
            "Team Beta",
            "Team Gamma",
            "Team Delta",
            "Team Echo",
            "Team Foxtrot"
        ];
        
        public static SeededPlayer[] SeededPlayers =>
        [
            new("Alice", Position.GoalKeeper),
            new("Bob", Position.Defender),
            new("Charlie", Position.GoalKeeper),
            new("Dana", Position.Midfielder) 
        ];
        
        public static SeededTeamStrength[] SeededTeamStrengths =>
        [
            new("Team Alpha", 5, 5, 4, 4),
            new("Team Beta", 1, 1, 2, 2)
            // All others default to 3,3,3,3 in the seeder
        ];
    }
}
