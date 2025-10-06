using FplDashboard.DataModel.Models;

namespace FplDashboard.API.IntegrationTests.Infrastructure.Models;

public class SeededData
{
    public List<Team> Teams { get; init; } = [];
    public GameWeek GameWeek { get; init; } = null!;
    public List<Player> Players { get; init; } = [];
}

public class DashboardSeededData : SeededData
{
    public List<GameWeek> FutureGameWeeks { get; init; } = [];
    public List<Fixture> Fixtures { get; init; } = [];
    public List<PlayerNews> PlayerNews { get; init; } = [];
    public List<TeamGameWeekData> TeamGameWeekData { get; init; } = [];
}

public record SeededPlayer(string Name, Position Position);

public record SeededTeamStrength(string TeamName, int AttackHome, int DefenceHome, int AttackAway, int DefenceAway);