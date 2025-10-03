using FplDashboard.DataModel.Models;

namespace FplDashboard.API.Tests.Infrastructure;

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
        public static string[] DefaultTeamNames => ["Team Alpha", "Team Beta"];
        
        public static SeededPlayer[] SeededPlayers =>
        [
            new() { Name = "Alice", Position = Position.GoalKeeper },
            new() { Name = "Bob", Position = Position.Defender },
            new() { Name = "Charlie", Position = Position.GoalKeeper },
            new() { Name = "Dana", Position = Position.Midfielder }
        ];
    }
}

public class SeededPlayer
{
    public string Name { get; init; } = string.Empty;
    public Position Position { get; init; }
}
