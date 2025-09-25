using FplDashboard.API.Queries;
using Microsoft.AspNetCore.Mvc;

namespace FplDashboard.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DashboardController(DashboardQueries dashboardQueries) : ControllerBase
{
    [HttpGet("dashboardData")]
    public async Task<IActionResult> GetDashboardData()
    {
        var dashboardData = await dashboardQueries.GetDashboardDataAsync();
        return Ok(dashboardData);
    }
}
