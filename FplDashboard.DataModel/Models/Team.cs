using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FplDashboard.DataModel.Models;

public class Team
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; init; }

    [MaxLength(30)]
    public string? Name { get; init; }

    [MaxLength(5)]
    public string? ShortName { get; init; }

    public List<Player> Players { get; init; } = [];
    
    public List<TeamGameWeekData> TeamGameWeekData { get; init; } = [];

    public List<Fixture> HomeFixtures { get; init; } = [];
    
    public List<Fixture> AwayFixtures { get; init; } = [];
}