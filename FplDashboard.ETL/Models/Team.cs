using System.Text.Json.Serialization;
using FplDashboard.DataModel.Models;
using TeamDataModel = FplDashboard.DataModel.Models.Team;
    
namespace FplDashboard.ETL.Models
{
    internal class Team
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("short_name")]
        public string? ShortName { get; init; }

        // Fixture difficulty ratings
        [JsonPropertyName("strength_overall_home")]
        public int StrengthOverallHome { get; set; }

        [JsonPropertyName("strength_overall_away")]
        public int StrengthOverallAway { get; init; }

        [JsonPropertyName("strength_attack_home")]
        public int StrengthAttackHome { get; set; }

        [JsonPropertyName("strength_attack_away")]
        public int StrengthAttackAway { get; set; }

        [JsonPropertyName("strength_defence_home")]
        public int StrengthDefenceHome { get; set; }

        [JsonPropertyName("strength_defence_away")]
        public int StrengthDefenceAway { get; set; }

        [JsonPropertyName("strength")]
        public int Strength { get; set; }
        
        [JsonPropertyName("points")]
        public int Points { get; set; }

        [JsonPropertyName("form")]
        public decimal? Form { get; set; }

        public static TeamDataModel GetTeamDataModelFromEtlModel(Team team) =>
            new()
            {
                Id = team.Id,
                Name = team.Name,
                ShortName = team.ShortName
            };

        public static TeamGameWeekData GetTeamGameWeekDataFromEtlModel(Team team, int gameWeekId) =>
            new()
            {
                TeamId = team.Id,
                GameWeekId = gameWeekId,
                StrengthOverallHome = team.StrengthOverallHome,
                StrengthOverallAway = team.StrengthOverallAway,
                StrengthAttackHome = team.StrengthAttackHome,
                StrengthAttackAway = team.StrengthAttackAway,
                StrengthDefenceHome = team.StrengthDefenceHome,
                StrengthDefenceAway = team.StrengthDefenceAway,
                Strength = team.Strength,
                Points = team.Points,
                Form = team.Form
            };
    }
}
