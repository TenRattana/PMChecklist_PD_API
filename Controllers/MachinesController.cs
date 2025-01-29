using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_machine")]
public class MachinesController : ControllerBase
{
    private readonly ILogger<MachinesController> _logger;

    public MachinesController(ILogger<MachinesController> logger)
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
