namespace FplDashboard.API.Features.Players;

public class PlayerPagedDto
{
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public double Cost { get; set; }
    public int Bonus { get; set; }
    public int TotalPoints { get; set; }
    public int Minutes { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int CleanSheets { get; set; }
    public double PointsPerGame { get; set; }
    public double Form { get; set; }
    public double ExpectedAssistsPer90 { get; set; }
    public double ExpectedGoalInvolvementsPer90 { get; set; }
    public double ExpectedGoalsPer90 { get; set; }
    public double ExpectedGoalsConcededPer90 { get; set; }
    public double DefensiveContributionPer90 { get; set; }
    public double SavesPer90 { get; set; }
    public double SelectedByPercent { get; set; }
    public double ValueSeason { get; set; }
    public double ValueForm { get; set; }
    public int Bps { get; set; }
    public double Influence { get; set; }
    public double Creativity { get; set; }
    public double Threat { get; set; }
    public double IctIndex { get; set; }
}

