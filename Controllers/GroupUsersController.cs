using Microsoft.AspNetCore.Mvc;
using PMChecklist_PD_API.Models;
using Microsoft.EntityFrameworkCore;

namespace PMChecklist_PD_API.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupUsersController : ControllerBase
{
    private readonly PCMhecklistContext _context;

    public GroupUsersController(PCMhecklistContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get All GroupUsers.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupUsers>>> GetProducts()
    {
        return await _context.GroupUsers.ToListAsync();
    }
}
