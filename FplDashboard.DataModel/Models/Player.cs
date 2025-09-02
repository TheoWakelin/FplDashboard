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

    public int TeamId { get; init; }
    
    public Team? Team { get; init; }

    public string Status { get; init; } = string.Empty;
    
    public double NowCost { get; init; }

    public Position Position { get; init; }

    public List<PlayerNews> News { get; init; } = [];
    
    public List<PlayerGameWeekData> PlayerGameWeekData { get; init; } = [];

    // Statistics
    public int Bonus { get; init; }
    
    public double PointsPerGame { get; init; }
    
    public int TotalPoints { get; init; }
    
    public int Minutes { get; init; }

    public int GoalsScored { get; init; }

    public int Assists { get; init; }

    public int CleanSheets { get; init; }

    public int GoalsConceded { get; init; }

    public int OwnGoals { get; init; }

    public int PenaltiesSaved { get; init; }

    public int PenaltiesMissed { get; init; }

    public int YellowCards { get; init; }

    public int RedCards { get; init; }

    public int Saves { get; init; }
}

public enum Position
{
    GoalKeeper = 1,
    Defender = 2,
    Midfielder = 3,
    Forward = 4
}