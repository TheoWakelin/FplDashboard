using Microsoft.AspNetCore.Mvc;

namespace FplDashboard.API.Features.Dashboard;

[ApiController]
[Route("[controller]")]
public class DashboardController(DashboardQueries dashboardQueries) : ControllerBase
{
    [HttpGet("dashboardData")]
    public async Task<IActionResult> GetDashboardData(CancellationToken cancellationToken)
    {
        var dashboardData = await dashboardQueries.GetDashboardDataAsync(cancellationToken);
        return Ok(dashboardData);
    }
}
