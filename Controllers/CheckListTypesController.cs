using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_login")]
public class CheckListTypesController : ControllerBase
{
    private readonly ILogger<CheckListTypesController> _logger;
    private readonly PCMhecklistContext _context;

    public CheckListTypesController(ILogger<CheckListTypesController> logger, PCMhecklistContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("/GetCheckListTypes")]
    public async Task<ActionResult<CheckListTypes>> GetCheckListTypes()
    {
        try
        {
            var data = await _context.CheckListTypes.Where(u => u.IsActive == true).ToListAsync();
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
