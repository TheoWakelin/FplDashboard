namespace FplDashboard.DataModel.Models;

public class TeamGameWeekData
{
    // Composite key: TeamId + GameWeekId (configured in DbContext)
    public int TeamId { get; init; }
    
    public Team? Team { get; init; }
    
    public int GameWeekId { get; init; }
    
    public GameWeek? GameWeek { get; init; }
    
    // Statistics
    public int StrengthOverallHome { get; set; }

    public int StrengthOverallAway { get; set; }

    public int StrengthAttackHome { get; set; }

    public int StrengthAttackAway { get; set; }

    public int StrengthDefenceHome { get; set; }

    public int StrengthDefenceAway { get; set; }

    public int Strength { get; set; }    

    public int Points { get; set; }

    public decimal? Form { get; set; }
}