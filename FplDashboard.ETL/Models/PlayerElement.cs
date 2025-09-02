using System.Text.Json.Serialization;
using FplDashboard.DataModel.Models;

namespace FplDashboard.ETL.Models
{
    public class PlayerElement
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("web_name")]
        public string? WebName { get; init; }

        [JsonPropertyName("status")]
        public string Status { get; init; } = string.Empty;

        [JsonPropertyName("news")]
        public string? News { get; init; }

        [JsonPropertyName("news_added")]
        public DateTime? NewsAdded { get; init; }

        [JsonPropertyName("now_cost")]
        public int NowCost { get; init; }

        [JsonPropertyName("chance_of_playing_this_round")]
        [JsonConverter(typeof(FlexibleNullableDoubleConverter))]
        public double? ChanceOfPlayingThisRound { get; init; }

        [JsonPropertyName("chance_of_playing_next_round")]
        [JsonConverter(typeof(FlexibleNullableDoubleConverter))]
        public double? ChanceOfPlayingNextRound { get; init; }

        [JsonPropertyName("expected_assists_per_90")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double ExpectedAssistsPer90 { get; init; }

        [JsonPropertyName("expected_goal_involvements_per_90")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double ExpectedGoalInvolvementsPer90 { get; init; }

        [JsonPropertyName("expected_goals_per_90")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double ExpectedGoalsPer90 { get; init; }

        [JsonPropertyName("expected_goals_conceded_per_90")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double ExpectedGoalsConcededPer90 { get; init; }

        [JsonPropertyName("defensive_contribution_per_90")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double DefensiveContributionPer90 { get; init; }

        [JsonPropertyName("saves_per_90")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double SavesPer90 { get; init; }

        [JsonPropertyName("minutes")] 
        public int Minutes { get; init; }

        [JsonPropertyName("goals_scored")] 
        public int GoalsScored { get; init; }

        [JsonPropertyName("assists")] 
        public int Assists { get; init; }

        [JsonPropertyName("clean_sheets")] 
        public int CleanSheets { get; init; }

        [JsonPropertyName("goals_conceded")] 
        public int GoalsConceded { get; init; }

        [JsonPropertyName("own_goals")] 
        public int OwnGoals { get; init; }

        [JsonPropertyName("penalties_saved")] 
        public int PenaltiesSaved { get; init; }

        [JsonPropertyName("penalties_missed")] 
        public int PenaltiesMissed { get; init; }

        [JsonPropertyName("yellow_cards")] 
        public int YellowCards { get; init; }

        [JsonPropertyName("red_cards")] 
        public int RedCards { get; init; }

        [JsonPropertyName("saves")] 
        public int Saves { get; init; }

        // Game stats
        [JsonPropertyName("total_points")] 
        public int TotalPoints { get; init; }

        [JsonPropertyName("event_points")] 
        public int EventPoints { get; init; }

        [JsonPropertyName("form")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double Form { get; init; }

        [JsonPropertyName("points_per_game")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double PointsPerGame { get; init; }

        [JsonPropertyName("selected_by_percent")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double SelectedByPercent { get; init; }

        [JsonPropertyName("value_season")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double ValueSeason { get; init; }

        [JsonPropertyName("value_form")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double ValueForm { get; init; }

        // Key stats
        [JsonPropertyName("bonus")] 
        public int Bonus { get; init; }

        [JsonPropertyName("bps")] 
        public int Bps { get; init; }

        [JsonPropertyName("influence")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double Influence { get; init; }

        [JsonPropertyName("creativity")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double Creativity { get; init; }

        [JsonPropertyName("threat")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double Threat { get; init; }

        [JsonPropertyName("ict_index")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double IctIndex { get; init; }

        // Relations
        [JsonPropertyName("team")] 
        public int Team { get; init; }

        [JsonPropertyName("element_type")] 
        public int ElementType { get; init; }

        public static Player GetPlayerFromEtlModel(PlayerElement player) =>
            new()
            {
                Id = player.Id,
                WebName = player.WebName,
                TeamId = player.Team,
                Status = player.Status,
                NowCost = player.NowCost,
                Position = (Position)player.ElementType,
                Bonus = player.Bonus,
                PointsPerGame = player.PointsPerGame,
                TotalPoints = player.TotalPoints,
                Minutes = player.Minutes,
                GoalsScored = player.GoalsScored,
                Assists = player.Assists,
                CleanSheets = player.CleanSheets,
                GoalsConceded = player.GoalsConceded,
                OwnGoals = player.OwnGoals,
                PenaltiesSaved = player.PenaltiesSaved,
                PenaltiesMissed = player.PenaltiesMissed,
                YellowCards = player.YellowCards,
                RedCards = player.RedCards,
                Saves = player.Saves
            };

        public static PlayerGameWeekData GetPlayerGameWeekDataFromEtlModel(PlayerElement player, int gameWeekId) =>
            new()
            {
                PlayerId = player.Id,
                GameWeekId = gameWeekId,
                NowCost = player.NowCost,
                ChanceOfPlayingThisRound = player.ChanceOfPlayingThisRound,
                ChanceOfPlayingNextRound = player.ChanceOfPlayingNextRound,
                ExpectedAssistsPer90 = player.ExpectedAssistsPer90,
                ExpectedGoalInvolvementsPer90 = player.ExpectedGoalInvolvementsPer90,
                ExpectedGoalsPer90 = player.ExpectedGoalsPer90,
                ExpectedGoalsConcededPer90 = player.ExpectedGoalsConcededPer90,
                DefensiveContributionPer90 = player.DefensiveContributionPer90,
                SavesPer90 = player.SavesPer90,
                EventPoints = player.EventPoints,
                Form = player.Form,
                SelectedByPercent = player.SelectedByPercent,
                ValueSeason = player.ValueSeason,
                ValueForm = player.ValueForm,
                Bps = player.Bps,
                Influence = player.Influence,
                Creativity = player.Creativity,
                Threat = player.Threat,
                IctIndex = player.IctIndex,
            };
        
        public static PlayerNews? GetPlayerNewsFromEtlModel(PlayerElement player)
        {
            if (string.IsNullOrWhiteSpace(player.News) || !player.NewsAdded.HasValue)
                return null;

            return new PlayerNews
            {
                PlayerId = player.Id,
                News = player.News,
                NewsAdded = player.NewsAdded.Value
            };
        }
    }
}