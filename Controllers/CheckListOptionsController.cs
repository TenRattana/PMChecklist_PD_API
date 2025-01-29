using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_checklist_option")]
public class CheckListOptionsController : ControllerBase
{
    private readonly ILogger<CheckListOptionsController> _logger;
    private readonly PCMhecklistContext _context;

    public CheckListOptionsController(ILogger<CheckListOptionsController> logger, PCMhecklistContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("/GetCheckListOptions/{page}/{pageSize}")]
    public ActionResult<object> GetCheckListOptions(int page, int pageSize)
    {
        try
        {
            var data = _context.CheckListOptions.FromSqlRaw("EXEC GetCheckListOptionInPage @PageIndex = {0} , @PageSize = {1}", page, pageSize).ToList();
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/SearchCheckListOptions/{Messages}")]
    public ActionResult<object> SearchCheckListOptions(string Messages)
    {
        
        try
        {
            var data = _context.CheckListOptions.FromSqlRaw("EXEC SearchCheckListOptionsWithPagination @SearchTerm = {0}", Messages).ToList();
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetCheckListOption/{GCLOptionID}")]
    public ActionResult<CheckListOptions> GetCheckListOption(string GCLOptionID)
    {
        try
        {
            var data = _context.CheckListOptions.FromSqlRaw("EXEC  GetCheckListOptionInPage @ID = {0}", GCLOptionID).ToList();
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
