using System.Text.Json.Serialization;
using FixtureInDb = FplDashboard.DataModel.Models.Fixture;

namespace FplDashboard.ETL.Models;

public class Fixture
{
    [JsonPropertyName("event")]
    public int GameweekId { get; init; }

    [JsonPropertyName("team_a")]
    public int TeamAId { get; init; }

    [JsonPropertyName("team_h")]
    public int TeamHId { get; init; }

    [JsonPropertyName("finished")]
    public bool Finished { get; set; }

    [JsonPropertyName("kickoff_time")]
    public DateTime? KickoffTime { get; set; }

    [JsonPropertyName("team_a_score")]
    public int? TeamAScore { get; set; }

    [JsonPropertyName("team_h_score")]
    public int? TeamHScore { get; set; }

    public FixtureInDb ToDataModelFixture(int eventId)
    {
        return new FixtureInDb
        {
            EventId = eventId,
            TeamAId = TeamAId,
            TeamHId = TeamHId,
            Finished = Finished,
            KickoffTime = KickoffTime,
            TeamAScore = TeamAScore,
            TeamHScore = TeamHScore
        };
    }
}
