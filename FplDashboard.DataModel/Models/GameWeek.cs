using System.ComponentModel.DataAnnotations;

namespace FplDashboard.DataModel.Models;

public class GameWeek
{
    [Key]
    public int Id { get; init; }

    public int GameWeekNumber { get; init; }

    public int YearSeasonStarted { get; init; }

    public DateTime DeadlineTime { get; init; }
    
    public GameWeekStatus Status { get; init; }

    public int? AverageEntryScore { get; init; }

    public int? HighestScore { get; init; }
    

    public List<PlayerGameWeekData> PlayerGameWeekData { get; init; } = [];
    
    public List<TeamGameWeekData> TeamGameWeekData { get; init; } = [];
}

public enum GameWeekStatus
{
    Previous,
    Current,
    Next,
    Future
}