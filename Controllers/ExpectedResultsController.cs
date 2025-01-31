using System.Dynamic;
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
            var data = _connection.QueryData<ExpectedResult>("EXEC GetExpectedResultInPage @PageIndex , @PageSize",  new { PageIndex = page, PageSize = pageSize });

            if (data == null || !data.Any())
            {
                return NotFound(new { status = false, message = "No data found." });
            }
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/SearchExpectedResults/{Messages}")]
    public ActionResult SearchExpectedResults(string Messages)
    {
        try
        {
            var data = _connection.QueryData<ExpectedResult>("EXEC SearchExpectedResultWithPagination @SearchTerm", new { SearchTerm = Messages });

            if (data == null || !data.Any())
            {
                return NotFound(new { status = false, message = "No data found." });
            }

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetExpectedResult/{TableID}")]
    public ActionResult GetExpectedResult(string TableID)
    {
        try
        {
            var data = _connection.QueryData<ExpectedTable>("EXEC GetExpectedResultTable @ID", new { ID = TableID });

            foreach (var item in data)
            {
                dynamic columns = item.DynamicColumns;
                foreach (var column in columns)
                {
                    Console.WriteLine($"{column.Key}: {column.Value}");
                }
            }

            if (data == null || !data.Any())
            {
                return NotFound(new { status = false, message = "No data found." });
            }

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

}

public class ExpectedTable
{
    public string? TableID { get; set; }
    public string? FormID { get; set; }
    public string? FormName { get; set; }
    public string? MachineID { get; set; }
    public string? MachineName { get; set; }
    public string? UserID { get; set; }
    public string? ApprovedID { get; set; }
    public bool Status { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? ApprovedTime { get; set; }
    public string? UserName { get; set; }
    public string? ApprovedName { get; set; }

    public ExpandoObject DynamicColumns { get; set; } = new ExpandoObject();
}
