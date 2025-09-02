using System.Text.Json.Serialization;

namespace FplDashboard.ETL.Models
{
    internal class WrapperFromApi
    {
        [JsonPropertyName("events")]
        public List<Event> Events { get; init; } = [];

        [JsonPropertyName("teams")]
        public List<Team> Teams { get; init; } = [];

        [JsonPropertyName("total_players")]
        public int TotalPlayers { get; init; }

        [JsonPropertyName("elements")]
        public List<PlayerElement> Elements { get; init; } = [];
    }
}
