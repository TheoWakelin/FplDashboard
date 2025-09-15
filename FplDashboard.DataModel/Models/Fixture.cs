using System.ComponentModel.DataAnnotations;

namespace FplDashboard.DataModel.Models;

public class Fixture
{
    [Key]
    public int Id { get; init; }

    public int EventId { get; init; }
    public GameWeek? GameWeek { get; init; }

    public int TeamAId { get; init; }
    public Team? TeamA { get; init; }

    public int TeamHId { get; init; }
    public Team? TeamH { get; init; }
    public bool Finished { get; set; }
    public DateTime? KickoffTime { get; set; }
    public int? TeamAScore { get; set; }
    public int? TeamHScore { get; set; }

    public void CopyMutablePropertiesFrom(Fixture source)
    {
        Finished = source.Finished;
        KickoffTime = source.KickoffTime;
        TeamAScore = source.TeamAScore;
        TeamHScore = source.TeamHScore;
    }
}
