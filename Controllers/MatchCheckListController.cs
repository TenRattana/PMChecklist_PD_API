using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_match_checklist_option")]
public class MatchCheckListController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public MatchCheckListController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet]
    public IActionResult GetGroupUsers()
    {
        return Ok("Group Users data");
    }
}
