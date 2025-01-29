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

 
    [HttpGet("/GetGroupCheckListOptions/{page}/{pageSize}")]
    public ActionResult<GroupCheckListOptions> GetGroupCheckListOptions(int page, int pageSize)
    {
         try
        {
            var data = _connection.QueryData<CheckLists>("EXEC GetCheckListInPage @PageIndex , @PageSize", new { page, pageSize });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/SearchGroupCheckLists/{Messages}")]
    public ActionResult<CheckLists> SearchGroupCheckLists(string Messages)
    {
          try
        {
            var data = _connection.QueryData<CheckListOptions>("EXEC SearchCheckListWithPagination @SearchTerm", new { SearchTerm = Messages });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetCheckList/{CListID}")]
    public ActionResult<CheckLists> GetCheckList(string CListID)
    {
          try
        {
            var data = _connection.QueryData<CheckListOptions>("EXEC GetCheckListInPage @ID", new { ID = CListID});

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetGroupCheckListOptionInForm/{CListIDS}")]
    public ActionResult<CheckLists> GetGroupCheckListOptionInForm(string CListIDS)
    {
         try
        {
            var data = _connection.QueryData<CheckListOptions>("EXEC GetCheckListInForm @ID", new { ID = CListIDS });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
