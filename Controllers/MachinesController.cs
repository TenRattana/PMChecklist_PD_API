using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_machine")]
public class MachinesController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public MachinesController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet]
    public IActionResult GetGroupUsers()
    {
        _logger.LogInformation("Access granted to GetGroupUsers API");
        return Ok("Group Users data");
    }
}
