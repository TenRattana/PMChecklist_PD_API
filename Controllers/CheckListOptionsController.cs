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
    public ActionResult<CheckListOptions> GetCheckListOptions(int page, int pageSize)
    {
        try
        {
            var data = _connection.QueryData<CheckListOptions>("EXEC GetCheckListOptionInPage @PageIndex , @PageSize", new { PageIndex = page, PageSize = pageSize });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/SearchCheckListOptions/{Messages}")]
    public ActionResult<CheckListOptions> SearchCheckListOptions(string Messages)
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
        if (string.IsNullOrEmpty(CLOptionID))
        {
            return BadRequest(new { status = false, message = "Invalid CLOptionID." });
        }

        var data = _connection.QueryData<CheckListOptions>("EXEC GetCheckListOptionInPage @ID = @CLOptionID", new { CLOptionID });

        if (data == null || !data.Any())
        {
            return NotFound(new { status = false, message = "No data found." });
        }

        return Ok(new { status = true, message = "Select success", data });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred with CLOptionID: {CLOptionID}. Exception: {ex.Message}"); 
        return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
    }
}

}
