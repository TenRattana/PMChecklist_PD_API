using Microsoft.AspNetCore.Mvc;
using PMChecklist_PD_API.Models;
using Microsoft.EntityFrameworkCore;
using PMChecklist_PD_API.Resources;

namespace PMChecklist_PD_API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly PCMhecklistContext _context;

    public UsersController(PCMhecklistContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsersResources>>> GetUsers()
    {
        var users = await _context.Users.Where(u => u.IsActive).Include(u => u.GroupUser).ToListAsync();

        if (users == null || !users.Any())
        {
            return NotFound(); 
        }

        var userDtos = users.Select(u => new UsersResources
        {
            UserID = u.UserID,
            UserName = u.UserName,
            GUserID = u.GUserID,
            GUserName = u.GroupUser.GUserName,
            IsActive = u.IsActive
        }).ToList();

        return Ok(userDtos);
    }


}
