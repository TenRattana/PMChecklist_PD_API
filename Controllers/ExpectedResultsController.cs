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
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public ExpectedResultsController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet("/GetExpectedResults/{page}/{pageSize}")]
    public IActionResult GetExpectedResults(int page, int pageSize)
    {
       try
        {
            var data = _connection.QueryData<ExpectedResult>("EXEC GetExpectedResultInPage @PageIndex , @PageSize", new { page, pageSize });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
