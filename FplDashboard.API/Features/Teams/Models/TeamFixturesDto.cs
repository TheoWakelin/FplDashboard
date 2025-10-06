namespace FplDashboard.API.Features.Teams.Models
{
    public class TeamFixturesDto
    {
        public string TeamName { get; set; } = string.Empty;
        public List<FixtureScoreDto> Fixtures { get; set; } = [];

        public int TotalAttackingStrength => Fixtures.Sum(f => f.AttackingStrength);
        public int TotalDefensiveStrength => Fixtures.Sum(f => f.DefensiveStrength);
        public int CumulativeStrength => TotalAttackingStrength + TotalDefensiveStrength;
    }

    public class FixtureScoreDto
    {
        public string TeamName { get; set; } = string.Empty;
        public string OpponentName { get; set; } = string.Empty;
        public int AttackingStrength { get; set; }
        public int DefensiveStrength { get; set; }
        public int GameWeekNumber { get; set; }
    }
}
