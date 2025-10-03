namespace FplDashboard.API.Features.Players;

public class PlayerPagedDto
{
    public int PlayerId { get; init; }
    public string PlayerName { get; init; } = string.Empty;
    public string TeamName { get; init; } = string.Empty;
    public int Position { get; init; }
    public double Cost { get; init; }
    public int Bonus { get; init; }
    public int TotalPoints { get; init; }
    public int Minutes { get; init; }
    public int Goals { get; init; }
    public int Assists { get; init; }
    public int CleanSheets { get; init; }
    public double PointsPerGame { get; init; }
    public double Form { get; init; }
    public double ExpectedAssistsPer90 { get; init; }
    public double ExpectedGoalInvolvementsPer90 { get; init; }
    public double ExpectedGoalsPer90 { get; init; }
    public double ExpectedGoalsConcededPer90 { get; init; }
    public double DefensiveContributionPer90 { get; init; }
    public double SavesPer90 { get; init; }
    public double SelectedByPercent { get; init; }
    public double ValueSeason { get; init; }
    public double ValueForm { get; init; }
    public int Bps { get; init; }
    public double Influence { get; init; }
    public double Creativity { get; init; }
    public double Threat { get; init; }
    public double IctIndex { get; init; }
}

