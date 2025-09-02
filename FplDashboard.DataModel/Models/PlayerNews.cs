using System.ComponentModel.DataAnnotations;

namespace FplDashboard.DataModel.Models;

public class PlayerNews
{
    // Composite key: PlayerId + NewsAdded (configured in DbContext)
    public int PlayerId { get; init; }
    
    public Player? Player { get; init; }
    
    [MaxLength(4000)]
    public string? News { get; init; }
    
    public DateTime? NewsAdded { get; init; }
}