using System.Text;
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
    private readonly LogService _logger;
    private readonly CheckListService _checkListService;

    public CheckListsController(Connection connection, CheckListService checkListService, LogService logger)
    {
        _logger = logger;
        _checkListService = checkListService;
        _connection = connection;
    }

    [HttpGet("/GetCheckLists/{page}/{pageSize}")]
    public ActionResult<CheckLists> GetCheckLists(int page, int pageSize)
    {
        try
        {
            var data = _connection.QueryData<CheckLists>("EXEC GetCheckListInPage @PageIndex , @PageSize", new { PageIndex = page, PageSize = pageSize });

            if (data == null || !data.Any()) return NotFound(new { status = false, message = "No data found." });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogActionError(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/SearchCheckLists/{Messages}")]
    public ActionResult<CheckLists> SearchCheckLists(string Messages)
    {
        try
        {
            var data = _connection.QueryData<CheckLists>("EXEC SearchCheckListWithPagination @SearchTerm", new { SearchTerm = Messages });

            if (data == null || !data.Any()) return NotFound(new { status = false, message = "No data found." });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogActionError(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetCheckList/{CListID}")]
    public ActionResult<CheckLists> GetCheckList(string CListID)
    {
        var errors = new List<string>();

        try
        {
            if (string.IsNullOrWhiteSpace(CListID)) errors.Add("The checklist id field is required.");

            if (errors.Any()) return BadRequest(new { errors });

            var data = _connection.QueryData<CheckLists>("EXEC GetCheckListInPage @ID = @CListID", new { CListID });

            if (data == null) return NotFound(new { status = false, message = "No data found." });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogActionError(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetCheckListInForm/{CListIDS}")]
    public ActionResult<CheckLists> GetCheckListInForm(string CListIDS)
    {
        try
        {
            var data = _connection.QueryData<CheckLists>("EXEC GetCheckListInForm @ID = @CListIDS", new { CListIDS });

            if (data == null || !data.Any()) return NotFound(new { status = false, message = "No data found." });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpPost("SaveCheckList")]
    public IActionResult SaveCheckList([FromBody] CheckLists checkList)
    {
        var logs = new StringBuilder();
        var errors = new List<string>();

        try
        {
            var exists = _connection.QueryData<dynamic>("SELECT CASE WHEN EXISTS (SELECT 1 FROM CheckLists WHERE CListName = @CListName AND CListID NOT IN (@CListID)) THEN 1 ELSE 0 END AS NameCount",
            new { CListID = checkList.CListID ?? "", CListName = checkList.CListName! }).FirstOrDefault();

            if (exists?.NameCount != null && Convert.ToInt32(exists!.NameCount) == 1)
            {
                errors.Add("The checklist name already exists.");
            }

            var CListID = checkList.CListID! ?? "";

            var data = _connection.QueryData<dynamic>("EXEC GetCheckListInPage @ID = @CListID", new { CListID }).FirstOrDefault();

            if (data != null)
            {
                var status = Convert.ToBoolean(data!.IsActive);
                var disable = Convert.ToBoolean(data.Disables);

                if (disable && status != checkList.IsActive)
                {
                    logs.AppendLine($"Disable : {disable} -> Can't change status.");
                    errors.Add("Change status not successful.");
                }
            }

            if (errors.Any())
            {
                logs.AppendLine("----------------------------------------------------");
                logs.AppendLine($"Checklist id : {checkList.CListID}");
                logs.AppendLine($"Checklist name : {checkList.CListName}");
                logs.AppendLine("----------------------------------------------------");

                _logger.LogError("Save Not Success : Checklist ", errors, logs);
                return BadRequest(new { errors = errors! });
            }

            var result = _checkListService.SaveCheckList(checkList, logs);

            if (result.Any(r => !r.Item2)) return StatusCode(500, new { message = result.FirstOrDefault()?.Item1, status = result.FirstOrDefault()?.Item2 });

            return Ok(new { message = "Status changed successfully.", logs = logs.ToString() });
        }
        catch (Exception ex)
        {
            _logger.LogActionError(ex);
            return StatusCode(500, new { message = "An error occurred while processing the request.", exception = ex.Message });
        }
    }

    [HttpPost("ChangeCheckList/{CListID}")]
    public IActionResult ChangeCheckList(string CListID)
    {
        var errors = new List<string>();
        var logs = new StringBuilder();
        bool status = false;

        try
        {
            if (string.IsNullOrWhiteSpace(CListID)) errors.Add("The checklist id field is required.");

            var data = _connection.QueryData<GroupMachines>("EXEC GetCheckListInPage @ID = @CListID", new { CListID }).FirstOrDefault();

            if (data == null) errors.Add("The checklist id field does not exist in the database.");

            else
            {
                var disable = Convert.ToBoolean(data.Disables);
                if (!disable)
                {
                    status = Convert.ToBoolean(data.IsActive);
                    logs.AppendLine("Change Status");
                    logs.AppendLine("----------------------------------------------------");

                    _logger.AppendObjectPropertiesToLog(ref logs, data!, new string[] { "CListID", "CListName", "IsActive" });
                }
                else errors.Add("Change status not successful.");
            }

            if (errors.Any()) return BadRequest(new { errors });

            var result = _checkListService.ChangeCheckList(CListID, status, logs);

            if (result.Any(r => !r.Item2)) return StatusCode(500, new { message = "Failed to change the status.", logs = logs.ToString() });

            return Ok(new { message = "Status changed successfully.", logs = logs.ToString() });
        }
        catch (Exception ex)
        {
            _logger.LogActionError(ex);
            return StatusCode(500, new { message = "An error occurred while processing the request.", exception = ex.Message });
        }
    }

    [HttpDelete("DeleteCheckList/{CListID}")]
    public IActionResult DeleteGroupMachine(string CListID)
    {
        var errors = new List<string>();
        var logs = new StringBuilder();

        try
        {
            if (string.IsNullOrWhiteSpace(CListID)) errors.Add("The checklist id field is required.");

            var data = _connection.QueryData<GroupMachines>("EXEC GetCheckListInPage @ID = @CListID", new { CListID }).FirstOrDefault();

            if (data == null) errors.Add("The checklist id field does not exist in the database.");

            else
            {
                var delete = Convert.ToBoolean(data.Deletes);
                if (!delete)
                {
                    logs.AppendLine("Delete Data");
                    logs.AppendLine("----------------------------------------------------");

                    _logger.AppendObjectPropertiesToLog(ref logs, data!, new string[] { "CListID", "CListName" });
                }
                else errors.Add("Delete not successful.");
            }

            if (errors.Any()) return BadRequest(new { errors });

            var result = _checkListService.DeleteCheckList(CListID, logs);

            if (result.Any(r => !r.Item2)) return StatusCode(500, new { message = "Failed to change the status.", logs = logs.ToString() });

            return Ok(new { message = "Status changed successfully.", logs = logs.ToString() });
        }
        catch (Exception ex)
        {
            _logger.LogActionError(ex);
            return StatusCode(500, new { message = "An error occurred while processing the request.", exception = ex.Message });
        }
    }
}
