using FplDashboard.DataModel.Models;
using System.Text.Json.Serialization;
using FplDashboard.ETL.Helpers;

namespace FplDashboard.ETL.Models
{
    public class Event
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("deadline_time")]
        public DateTime DeadlineTime { get; init; }

        [JsonPropertyName("is_previous")]
        public bool IsPrevious { get; init; }

        [JsonPropertyName("is_current")]
        public bool IsCurrent { get; init; }

        [JsonPropertyName("is_next")]
        public bool IsNext { get; init; }
        
        [JsonPropertyName("average_entry_score")]
        public int? AverageEntryScore { get; init; }

        [JsonPropertyName("highest_score")]
        public int? HighestScore { get; init; }
        
        public static GameWeek GetGameWeekFromEtlModel(Event gameWeek) =>
            new()
            {
                GameWeekNumber = gameWeek.Id,
                DeadlineTime = gameWeek.DeadlineTime,
                Status = gameWeek.IsCurrent ? GameWeekStatus.Current :
                         gameWeek.IsNext ? GameWeekStatus.Next :
                         gameWeek.IsPrevious ? GameWeekStatus.Previous :
                         gameWeek.AverageEntryScore != 0 ? GameWeekStatus.Finished :
                         GameWeekStatus.Future,
                AverageEntryScore = gameWeek.AverageEntryScore,
                HighestScore = gameWeek.HighestScore,
                YearSeasonStarted = YearHelpers.GetYearCurrentSeasonStarted()
            };
    }
}
