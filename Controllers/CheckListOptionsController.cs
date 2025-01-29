using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_checklist_option")]
public class CheckListOptionsController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public CheckListOptionsController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet("/GetCheckListOptions/{page}/{pageSize}")]
    public ActionResult<object> GetCheckListOptions(int page, int pageSize)
    {
        try
        {
            var data = _connection.QueryData<CheckListOptions>("EXEC GetCheckListOptionInPage @PageIndex , @PageSize", new { page, pageSize });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/SearchCheckListOptions/{Messages}")]
    public ActionResult<object> SearchCheckListOptions(string Messages)
    {

        try
        {
            var data = _connection.QueryData<CheckListOptions>("EXEC SearchCheckListOptionsWithPagination @SearchTerm", new { SearchTerm = Messages });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetCheckListOption/{CLOptionID}")]
    public ActionResult<CheckListOptions> GetCheckListOption(string CLOptionID)
    {
        try
        {
            var data = _connection.QueryData<CheckListOptions>("EXEC GetCheckListOptionInPage @ID", new { ID = CLOptionID });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
