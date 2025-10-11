using System.ComponentModel.DataAnnotations;

namespace FplDashboard.API.Features.Players.Models;

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
    
    public bool ShouldCacheFilters()
    {
        return IsStandardPage() && (IsDefaultView() ||
               HasOnlyPositionFilter() ||
               HasOnlyTeamFilter());  
    }
    
    private bool IsStandardPage() => Page == 1 && PageSize == 20;

    public string GenerateCacheKey()
    {
        var keyParts = new List<string> { "players" };

        if (PositionIds is not null && PositionIds.Count > 0)
            keyParts.Add($"pos_{string.Join(",", PositionIds.OrderBy(x => x))}");
    
        if (TeamIds is not null && TeamIds.Count > 0)
            keyParts.Add($"team_{string.Join(",", TeamIds.OrderBy(x => x))}");

        return string.Join("_", keyParts);
    }
    
    private bool IsDefaultView()
    {
        return FilterChecker.For(this)
            .HasNoPositionFilter()
            .HasNoTeamFilter()
            .HasNoPlayerNameFilter()
            .HasNoMinutesFilter()
            .HasNoOrderBy()
            .HasDefaultPage()
            .IsMatch();
    }

    private bool HasOnlyPositionFilter()
    {
        return FilterChecker.For(this)
            .HasPositionFilter()
            .HasNoTeamFilter()
            .HasNoPlayerNameFilter()
            .HasNoMinutesFilter()
            .HasNoOrderBy()
            .HasDefaultPage()
            .IsMatch();
    }

    private bool HasOnlyTeamFilter()
    {
        return FilterChecker.For(this)
            .HasNoPositionFilter()
            .HasTeamFilter()
            .HasNoPlayerNameFilter()
            .HasNoMinutesFilter()
            .HasNoOrderBy()
            .HasDefaultPage()
            .IsMatch();
    }
}
