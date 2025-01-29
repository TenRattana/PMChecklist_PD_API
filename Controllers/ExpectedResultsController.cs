using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_expected_result")]
public class ExpectedResultsController : ControllerBase
{
    private readonly ILogger<ExpectedResultsController> _logger;
    private readonly PCMhecklistContext _context;

    public ExpectedResultsController(ILogger<ExpectedResultsController> logger , PCMhecklistContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("/GetExpectedResults/{page}/{pageSize}")]
    public IActionResult GetExpectedResults(int page, int pageSize)
    {
        try
        {
            var data = _context.ExpectedResult.FromSqlRaw("EXEC GetExpectedResultInPage @PageIndex = {0} , @PageSize = {1}", page, pageSize).ToList();
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
