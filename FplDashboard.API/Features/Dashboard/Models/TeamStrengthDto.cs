namespace FplDashboard.API.Features.Dashboard.Models;

public record TeamStrengthDto(string TeamName, int CumulativeStrength, TeamStrengthCategory Category);

public enum TeamStrengthCategory
{
    Top = 0,
    Bottom = 1
}
