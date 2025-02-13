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
    private readonly Connection _connection;

    public ApprovedsController(Connection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Select a data Approved from database with page and pagesize.
    /// </summary>
    [HttpGet("GetApproveds/{page}/{pageSize}")]
    public ActionResult GetApproveds(int page, int pageSize)
    {
        try
        {
            var data = _connection.QueryData<Approveds>("EXEC GetApprovedInPage @PageIndex , @PageSize", new { PageIndex = page, PageSize = pageSize });

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

    /// <summary>
    /// Select a data Approved from database with searching.
    /// </summary>
    [HttpGet("SearchApproveds/{Messages}")]
    public ActionResult SearchApproveds(string Messages)
    {
        try
        {
            var data = _connection.QueryData<Approveds>("EXEC SearchApprovedWithPagination @SearchTerm", new { SearchTerm = Messages });

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
