using Microsoft.AspNetCore.Mvc;

namespace FplDashboard.API.Features.Teams
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController(TeamsQueries teamsQueries) : ControllerBase
    {
        [HttpGet("fixtures")]
        public async Task<IActionResult> GetTeamFixtures(CancellationToken cancellationToken)
        {
            var teamFixtures = await teamsQueries.GetTeamFixturesAsync(cancellationToken);
            return Ok(teamFixtures);
        }
    }
}
