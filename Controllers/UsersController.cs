using Microsoft.AspNetCore.Mvc;
using PMChecklist_PD_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace PMChecklist_PD_API.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize(Roles = "SuperAdmin")]
public class UsersController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public UsersController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetUsers()
    {
        var users = await _context.Users.Where(u => u.IsActive).Include(u => u.GroupUser).ToListAsync();

        if (users == null || !users.Any())
        {
            return NotFound();
        }

        var userDtos = _common.GenerateJwtToken(users.First().UserName!, ["SuperAdmin"]);

        return Ok(new { userDtos });
    }


}
