using Microsoft.AspNetCore.Mvc;

namespace FplDashboard.API.Features.Players;

[ApiController]
[Route("[controller]")]
public class PlayersController(PlayersQueries playersQueries) : ControllerBase
{
    [HttpGet("paged")]
    public async Task<IActionResult> GetPagedPlayers(
        [FromQuery] int? teamId,
        [FromQuery] string? position,
        [FromQuery] string? orderBy,
        [FromQuery] string? orderDir,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var players = await playersQueries.GetPagedPlayersAsync(
            teamId,
            position,
            orderBy,
            orderDir,
            page,
            pageSize,
            cancellationToken);
        return Ok(players);
    }
}
