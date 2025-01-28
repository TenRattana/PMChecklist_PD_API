using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize(Policy = "SuperAdmins")]
// [CustomRoleAuthorize("SuperAdmin")]
public class GroupUsersController : ControllerBase
{
    private readonly ILogger<GroupUsersController> _logger;

    public GroupUsersController(ILogger<GroupUsersController> logger)
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
