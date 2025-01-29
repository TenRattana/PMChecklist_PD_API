using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_login")]
public class TimeSchedulesController : ControllerBase
{
    private readonly ILogger<TimeSchedulesController> _logger;

    public TimeSchedulesController(ILogger<TimeSchedulesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetGroupUsers()
    {
        _logger.LogInformation("Access granted to GetGroupUsers API");
        return Ok("Group Users data");
    }
}
