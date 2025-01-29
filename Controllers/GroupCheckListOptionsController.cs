using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_checklist_group")]
public class GroupCheckListOptionsController : ControllerBase
{
    private readonly ILogger<GroupCheckListOptionsController> _logger;

    public GroupCheckListOptionsController(ILogger<GroupCheckListOptionsController> logger)
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
