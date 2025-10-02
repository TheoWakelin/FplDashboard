using System.ComponentModel.DataAnnotations;

namespace FplDashboard.API.Features.Players;

public class PlayerFilterRequest
{
    /// <summary>
    /// Filter by team IDs. If null or empty, no team filtering is applied.
    /// </summary>
    public List<int>? TeamIds { get; set; }

    /// <summary>
    /// Filter by position IDs. If null or empty, no position filtering is applied.
    /// </summary>
    public List<int>? PositionIds { get; set; }

    /// <summary>
    /// Search for players by name (partial match, case-insensitive). If null or empty, no name filtering is applied.
    /// </summary>
    public string? PlayerName { get; set; }

    /// <summary>
    /// Minimum number of minutes played. If null, no minutes filtering is applied.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Minimum minutes must be 0 or greater")]
    public int? MinMinutes { get; set; }

    /// <summary>
    /// Column to order by. Must be one of the allowed columns.
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Order direction. Either "ASC" or "DESC". Defaults to "DESC".
    /// </summary>
    public string? OrderDir { get; set; }

    /// <summary>
    /// Page number for pagination. Must be 1 or greater.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page must be 1 or greater")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page. Must be between 1 and 100.
    /// </summary>
    [Range(1, 200, ErrorMessage = "Page size must be between 1 and 200")]
    public int PageSize { get; set; } = 20;
}
