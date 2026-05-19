using Microsoft.AspNetCore.Mvc;

using HireConnect.Analytic.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace HireConnect.Analytics.Controllers;

[ApiController]
[Route("api/analytics")]
[Authorize]
public class DashboardController
    : ControllerBase
{
    private readonly IDashboardService
        _dashboardService;

    public DashboardController(
        IDashboardService dashboardService
    )
    {
        _dashboardService =
            dashboardService;
    }

    [Authorize(Roles = "Recruiter")]
    [HttpGet("recruiter")]
    public async Task<ActionResult>
    GetRecruiterDashboard()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized("User ID not found in token.");
        }

        int recruiterId = int.Parse(userIdClaim);
        var dashboard =
            await _dashboardService
                .GetRecruiterDashboardAsync(recruiterId);

        return Ok(dashboard);
    }
}