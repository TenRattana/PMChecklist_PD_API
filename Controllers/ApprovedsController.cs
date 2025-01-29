using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_approved")]
public class ApprovedsController : ControllerBase
{
    private readonly ILogger<ApprovedsController> _logger;
    private readonly PCMhecklistContext _context;

    public ApprovedsController(ILogger<ApprovedsController> logger, PCMhecklistContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("/GetApproveds/{page}/{pageSize}")]
    public IActionResult GetApproveds(int page, int pageSize)
    {
        try
        {
            var data = _context.ExpectedResult.FromSqlRaw("EXEC GetApprovedInPage @PageIndex = {0} , @PageSize = {1}", page, pageSize).ToList();
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
