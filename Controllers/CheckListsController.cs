using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_checklist")]
public class CheckListsController : ControllerBase
{
    private readonly ILogger<CheckListsController> _logger;
    private readonly PCMhecklistContext _context;

    public CheckListsController(ILogger<CheckListsController> logger, PCMhecklistContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("/GetCheckLists/{page}/{pageSize}")]
    public ActionResult<CheckLists> GetCheckLists(int page, int pageSize)
    {
        try
        {
            var data = _context.CheckLists.FromSqlRaw("EXEC GetCheckListInPage @PageIndex = {0} , @PageSize = {1}", page, pageSize).ToList();
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/SearchCheckLists/{Messages}")]
    public ActionResult<CheckLists> SearchCheckLists(string Messages)
    {
        
        try
        {
            var data = _context.CheckLists.FromSqlRaw("EXEC SearchCheckListWithPagination @SearchTerm = {0}", Messages).ToList();
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetCheckList/{CListID}")]
    public ActionResult<CheckLists> GetCheckList(string CListID)
    {
        try
        {
            var data = _context.CheckLists.FromSqlRaw("EXEC GetCheckListInPage @ID = {0}", CListID).ToList();
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetCheckListInForm/{Group_CListID}")]
    public ActionResult<CheckLists> GetCheckListInForm(string Group_CListID)
    {
        try
        {
            var data = _context.CheckLists.FromSqlRaw("EXEC GetCheckListInForm @ID = {0}", Group_CListID).ToList();
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
