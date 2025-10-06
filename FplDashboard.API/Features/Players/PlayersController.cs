using FplDashboard.API.Features.Players.Models;
using Microsoft.AspNetCore.Mvc;

namespace FplDashboard.API.Features.Players;

[ApiController]
[Route("[controller]")]
public class PlayersController(PlayersQueries playersQueries) : ControllerBase
{
    [HttpPost("paged")]
    public async Task<IActionResult> GetPagedPlayers(
        [FromBody] PlayerFilterRequest request,
        CancellationToken cancellationToken = default)
    {
        var players = await playersQueries.GetPagedPlayersAsync(
            request,
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
