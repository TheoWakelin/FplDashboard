namespace FplDashboard.DataModel.Models;

public class PlayerGameWeekData
{
    // Composite key: PlayerId + GameWeekId (configured in DbContext)
    public int PlayerId { get; init; }
    
    public Player? Player { get; init; }

    public int GameWeekId { get; init; }
    
    public GameWeek? GameWeek { get; init; }
    
    // Statistics
    public double NowCost { get; set; }

    public double? ChanceOfPlayingThisRound { get; set; }

    public double? ChanceOfPlayingNextRound { get; set; }

    public double ExpectedAssistsPer90 { get; set; }

    public double ExpectedGoalInvolvementsPer90 { get; set; }

    public double ExpectedGoalsPer90 { get; set; }

    public double ExpectedGoalsConcededPer90 { get; set; }

    public double DefensiveContributionPer90 { get; set; }

    public double SavesPer90 { get; set; }

    public int EventPoints { get; set; }

    public double Form { get; set; }

    public double SelectedByPercent { get; set; }

    public double ValueSeason { get; set; }

    public double ValueForm { get; set; }

    public int Bps { get; set; }

    public double Influence { get; set; }

    public double Creativity { get; set; }

    public double Threat { get; set; }

    public double IctIndex { get; set; }
}