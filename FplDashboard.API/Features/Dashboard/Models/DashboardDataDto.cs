namespace FplDashboard.API.Features.Dashboard.Models
{
    public class DashboardDataDto
    {
        public IEnumerable<PlayerNewsDto> PlayerNews { get; set; } = new List<PlayerNewsDto>();
        public IEnumerable<TeamStrengthDto> TopTeams { get; set; } = new List<TeamStrengthDto>();
        public IEnumerable<TeamStrengthDto> BottomTeams { get; set; } = new List<TeamStrengthDto>();
    }
}

