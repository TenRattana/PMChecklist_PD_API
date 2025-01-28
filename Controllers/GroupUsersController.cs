using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize(Roles = "SuperAdmin")]
public class GroupUsersController : ControllerBase
{
    private readonly ILogger<GroupUsersController> _logger;

    public GroupUsersController(ILogger<GroupUsersController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetGroupUsers()
    {
        _logger.LogInformation("Access granted to GetGroupUsers API");

        return Ok("Group Users data");
    }
}
