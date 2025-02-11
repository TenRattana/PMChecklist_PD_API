using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_checklist_group")]
public class GroupCheckListOptionsController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public GroupCheckListOptionsController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }


    [HttpGet("GetGroupCheckListOptions/{page}/{pageSize}")]
    public ActionResult<GroupCheckListOptions> GetGroupCheckListOptions(int page, int pageSize)
    {
        try
        {
            var data = _connection.QueryData<GroupCheckListOptions>("EXEC GetGroupCheckListOptionInPage @PageIndex , @PageSize", new { PageIndex = page, PageSize = pageSize });

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

    [HttpGet("SearchGroupCheckLists/{Messages}")]
    public ActionResult<CheckLists> SearchGroupCheckLists(string Messages)
    {
        try
        {
            var data = _connection.QueryData<GroupCheckListOptions>("EXEC SearchGroupCheckListOptionsWithPagination @SearchTerm", new { SearchTerm = Messages });

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

    [HttpGet("GetGroupCheckListOption/{GCLOptionID}")]
    public ActionResult<CheckLists> GetGroupCheckListOption(string GCLOptionID)
    {
        try
        {
            var data = _connection.QueryData<GroupCheckListOptions>("EXEC GetGroupCheckListOptionInPage @ID = @GCLOptionID", new { GCLOptionID });

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

    [HttpGet("GetGroupCheckListOptionInForm/{GCLOptionIDS}")]
    public ActionResult<CheckLists> GetGroupCheckListOptionInForm(string[] GCLOptionIDS)
    {
        try
        {
            var data = _connection.QueryData<GroupCheckListOptions>("EXEC GetGroupCheckListOptionInForm @ID", new { ID = GCLOptionIDS });

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
