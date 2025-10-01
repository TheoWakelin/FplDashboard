using Microsoft.AspNetCore.Mvc;

namespace FplDashboard.API.Features.Players;

[ApiController]
[Route("[controller]")]
public class PlayersController(PlayersQueries playersQueries) : ControllerBase
{
    [HttpGet("paged")]
    public async Task<IActionResult> GetPagedPlayers(
        [FromQuery] List<int>? teamIds,
        [FromQuery] string? position,
        [FromQuery] string? orderBy,
        [FromQuery] string? orderDir,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var players = await playersQueries.GetPagedPlayersAsync(
            teamIds,
            position,
            orderBy,
            orderDir,
            page,
            pageSize,
            cancellationToken);
        return Ok(players);
    }

    [HttpGet("teams")]
    public async Task<IActionResult> GetAllTeams(CancellationToken cancellationToken = default)
    {
        var teams = await playersQueries.GetAllTeamsAsync(cancellationToken);
        return Ok(teams);
    }
}
