namespace FplDashboard.API.Features.Dashboard.Models
{
    public class PlayerNewsDto
    {
        public DateTime NewsAdded { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public string News { get; set; } = string.Empty;
    }
}