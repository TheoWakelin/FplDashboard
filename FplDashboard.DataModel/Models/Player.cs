using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FplDashboard.DataModel.Models;

public class Player
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; init; }

    [MaxLength(30)]
    public string? WebName { get; init; }

    public int TeamId { get; set; }
    
    public Team? Team { get; set; }
    
    public Position Position { get; set; }

    public string Status { get; set; } = string.Empty;
    
    public double NowCost { get; set; }

    public List<PlayerNews> News { get; set; } = [];
    
    public List<PlayerGameWeekData> PlayerGameWeekData { get; set; } = [];

    // Statistics
    public int Bonus { get; set; }
    
    public double PointsPerGame { get; set; }
    
    public int TotalPoints { get; set; }
    
    public int Minutes { get; set; }

    public int GoalsScored { get; set; }

    public int Assists { get; set; }

    public int CleanSheets { get; set; }

    public int GoalsConceded { get; set; }

    public int OwnGoals { get; set; }

    public int PenaltiesSaved { get; set; }

    public int PenaltiesMissed { get; set; }

    public int YellowCards { get; set; }

    public int RedCards { get; set; }

    public int Saves { get; set; }
}

public enum Position
{
    GoalKeeper = 1,
    Defender = 2,
    Midfielder = 3,
    Forward = 4
}