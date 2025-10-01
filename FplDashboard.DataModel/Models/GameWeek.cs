using System.ComponentModel.DataAnnotations;

namespace FplDashboard.DataModel.Models;

public class GameWeek
{
    [Key]
    public int Id { get; init; }

    public int GameWeekNumber { get; init; }

    public int YearSeasonStarted { get; init; }

    public DateTime DeadlineTime { get; set; }
    
    public GameWeekStatus Status { get; set; }

    public int? AverageEntryScore { get; set; }

    public int? HighestScore { get; set; }
    
    public List<PlayerGameWeekData> PlayerGameWeekData { get; init; } = [];
    
    public List<TeamGameWeekData> TeamGameWeekData { get; init; } = [];

    public List<Fixture> Fixtures { get; init; } = [];
}

public enum GameWeekStatus
{
    Previous,
    Current,
    Next,
    Future,
    Finished
}