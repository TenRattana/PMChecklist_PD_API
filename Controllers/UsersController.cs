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
    private readonly PCMhecklistContext _context;
    private readonly Common _common;

    public UsersController(PCMhecklistContext context , Common common)
    {
        _context = context;
        _common = common;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetUsers()
    {
        var users = await _context.Users.Where(u => u.IsActive).Include(u => u.GroupUser).ToListAsync();

        if (users == null || !users.Any())
        {
            return NotFound(); 
        }

        var userDtos = _common.GenerateJwtToken(users.First().UserName!, "SuperAdmin");

        return Ok(new { userDtos });
    }


}
