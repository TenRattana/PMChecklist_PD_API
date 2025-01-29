using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_match_checklist_option")]
public class MatchCheckListOptionController : ControllerBase
{
    private readonly ILogger<MatchCheckListOptionController> _logger;

    public MatchCheckListOptionController(ILogger<MatchCheckListOptionController> logger)
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
