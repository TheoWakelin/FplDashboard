using System.ComponentModel.DataAnnotations;

namespace FplDashboard.DataModel.Models;

public class Fixture
{
    [Key]
    public int Id { get; init; }

    public int GameweekId { get; init; }
    public GameWeek? GameWeek { get; init; }

    public int AwayTeamId { get; init; }
    public Team? AwayTeam { get; init; }

    public int HomeTeamId { get; init; }
    public Team? HomeTeam { get; init; }
    public bool Finished { get; set; }
    public DateTime? KickoffTime { get; set; }
    public int? AwayTeamScore { get; set; }
    public int? HomeTeamScore { get; set; }

    public void CopyMutablePropertiesFrom(Fixture source)
    {
        Finished = source.Finished;
        KickoffTime = source.KickoffTime;
        AwayTeamScore = source.AwayTeamScore;
        HomeTeamScore = source.HomeTeamScore;
    }
}
