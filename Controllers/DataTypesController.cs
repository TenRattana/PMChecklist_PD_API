using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_login")]
public class DataTypesController : ControllerBase
{
    private readonly ILogger<DataTypesController> _logger;
    private readonly PCMhecklistContext _context;

    public DataTypesController(ILogger<DataTypesController> logger ,PCMhecklistContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("/GetDataTypes")]
    public async Task<ActionResult<DataTypes>> GetDataTypes()
    {
        try
        {
            var data = await _context.DataTypes.Where(u => u.IsActive == true).ToListAsync();
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
