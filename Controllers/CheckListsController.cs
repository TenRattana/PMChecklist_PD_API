using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_checklist")]
public class CheckListsController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public CheckListsController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet("/GetCheckLists/{page}/{pageSize}")]
    public ActionResult<CheckLists> GetCheckLists(int page, int pageSize)
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

    [HttpGet("/SearchCheckLists/{Messages}")]
    public ActionResult<CheckLists> SearchCheckLists(string Messages)
    {
          try
        {
            var data = _connection.QueryData<CheckLists>("EXEC SearchCheckListWithPagination @SearchTerm", new { SearchTerm = Messages });

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
            var data = _connection.QueryData<CheckLists>("EXEC GetCheckListInPage @ID", new { ID = CListID});

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetCheckListInForm/{CListIDS}")]
    public ActionResult<CheckLists> GetCheckListInForm(string CListIDS)
    {
         try
        {
            var data = _connection.QueryData<CheckLists>("EXEC GetCheckListInForm @ID", new { ID = CListIDS });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
