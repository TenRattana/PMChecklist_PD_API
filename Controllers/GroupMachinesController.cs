using System.Text;
using Microsoft.AspNetCore.Mvc;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_machine_group")]
public class GroupMachinesController : ControllerBase
{
    private readonly Connection _connection;
    private readonly GroupMachineService _groupMachineService;
    private readonly LogService _logService;

    public GroupMachinesController(Connection connection, GroupMachineService groupMachineService, LogService logService)
    {
        _logService = logService;
        _groupMachineService = groupMachineService;
        _connection = connection;
    }

    [HttpGet("/GetGroupMachines/{page}/{pageSize}")]
    public ActionResult<GroupMachines> GetGroupMachines(int page, int pageSize)
    {
        try
        {
            var data = _connection.QueryData<GroupMachines>("EXEC GetGroupMachinesInPage @PageIndex , @PageSize", new { PageIndex = page, PageSize = pageSize }).ToList();

            if (data == null || !data.Any()) return NotFound(new { status = false, message = "No data found." });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logService.LogError("Error occurred while fetching group machines.", ex.ToString());
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/SearchGroupMachines/{Messages}")]
    public ActionResult<GroupMachines> SearchGroupMachines(string Messages)
    {
        try
        {
            var data = _connection.QueryData<GroupMachines>("EXEC SearchGroupMachinesWithPagination @SearchTerm", new { SearchTerm = Messages }).ToList();

            if (data == null || !data.Any()) return NotFound(new { status = false, message = "No data found." });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logService.LogError("Error occurred while searching group machines.", ex.ToString());
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetGroupMachine/{GMachineID}")]
    public ActionResult<GroupMachines> GetGroupMachine(string GMachineID)
    {
        var errors = new List<string>();

        try
        {
            if (string.IsNullOrWhiteSpace(GMachineID)) errors.Add("The group machine id field is required.");

            if (errors.Any()) return BadRequest(new { errors });

            var data = _connection.QueryData<GroupMachines>("EXEC GetGroupMachinesInPage @ID = @GMachineID", new { GMachineID }).FirstOrDefault();

            if (data == null) return NotFound(new { status = false, message = "No data found." });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logService.LogError("Error occurred while fetching group machine.", ex.ToString());
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpPost("SaveGroupMachine")]
    public IActionResult SaveGroupMachine([FromBody] GroupMachines groupMachine)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { errors });
        }

        try
        {
            var exists = _connection.QueryData<dynamic>("SELECT CASE WHEN EXISTS (SELECT 1 FROM GroupMachines WHERE GMachineName = @GMachineName AND GMachineID NOT IN (@GMachineID)) THEN 1 ELSE 0 END AS NameCount",
            new { GMachineID = groupMachine.GMachineID ?? "", GMachineName = groupMachine.GMachineName! }).FirstOrDefault();

            if (exists != null && Convert.ToInt32(exists!.NameCount) == 1) return BadRequest(new { message = "The group machine name already exists." });

            var data = _connection.QueryData<dynamic>("EXEC GetGroupMachinesInPage @ID = @GMachineID", new { GMachineID = groupMachine.GMachineID ?? "" }).FirstOrDefault();

            if (data != null && Convert.ToInt32(data!.NameCount) == 1)
            {
                var Status = Convert.ToBoolean(data!.IsActive);

                if (Status != groupMachine.IsActive) return BadRequest(new { message = "Change ststus not successful.." });
            }

            return Ok(new
            {
                groupMachine.GMachineID,
                groupMachine.GMachineName,
                groupMachine.Description,
                groupMachine.IsActive
            });
        }
        catch (Exception ex)
        {
            _logService.LogError("Error occurred while changing group machine status.", ex.ToString());
            return StatusCode(500, new { message = "An error occurred while processing the request.", exception = ex.Message });
        }

    }

    [HttpPost("ChangeGroupMachine/{GMachineID}")]
    public IActionResult ChangeGroupMachine(string GMachineID)
    {
        var errors = new List<string>();
        var logs = new StringBuilder();
        bool status = false;

        try
        {
            if (string.IsNullOrWhiteSpace(GMachineID)) errors.Add("The group machine id field is required.");

            var data = _connection.QueryData<GroupMachines>("EXEC GetGroupMachinesInPage @ID = @GMachineID", new { GMachineID }).FirstOrDefault();

            if (data == null) errors.Add("The group machine id field does not exist in the database.");

            else
            {
                var disable = Convert.ToBoolean(data.Disables);
                if (!disable)
                {
                    status = Convert.ToBoolean(data.IsActive);
                    logs.AppendLine("Change Status");
                    logs.AppendLine("----------------------------------------------------");

                    string[] propertiesToLog = { "GMachineID", "GMachineName", "IsActive" };
                    _logService.AppendObjectPropertiesToLog(ref logs, data, propertiesToLog);
                }
                else errors.Add("Change status not successful.");
            }

            if (errors.Any()) return BadRequest(new { errors });

            var result = _groupMachineService.ChangeGroupMachine(GMachineID, status, logs);

            if (result.Any(r => !r.Item2)) return StatusCode(500, new { message = "Failed to change the status.", logs = logs.ToString() });

            return Ok(new { message = "Status changed successfully.", logs = logs.ToString() });
        }
        catch (Exception ex)
        {
            _logService.LogError("Error occurred while changing group machine status.", ex.ToString());
            return StatusCode(500, new { message = "An error occurred while processing the request.", exception = ex.Message });
        }
    }

    [HttpDelete("DeleteGroupMachine/{GMachineID}")]
    public IActionResult DeleteGroupMachine(string GMachineID)
    {
        var errors = new List<string>();
        var logs = new StringBuilder();

        try
        {
            if (string.IsNullOrWhiteSpace(GMachineID)) errors.Add("The group machine id field is required.");

            var data = _connection.QueryData<GroupMachines>("EXEC GetGroupMachinesInPage @ID = @GMachineID", new { GMachineID }).FirstOrDefault();

            if (data == null) errors.Add("The group machine id field does not exist in the database.");

            else
            {
                var delete = Convert.ToBoolean(data.Deletes);
                if (!delete)
                {
                    logs.AppendLine("Delete Data");
                    logs.AppendLine("----------------------------------------------------");

                    string[] propertiesToLog = { "GMachineID", "GMachineName" };
                    _logService.AppendObjectPropertiesToLog(ref logs, data, propertiesToLog);
                }
                else errors.Add("Delete not successful.");
            }

            if (errors.Any()) return BadRequest(new { errors });

            var result = _groupMachineService.DeleteGroupMachine(GMachineID, logs);

            if (result.Any(r => !r.Item2)) return StatusCode(500, new { message = "Failed to change the status.", logs = logs.ToString() });

            return Ok(new { message = "Status changed successfully.", logs = logs.ToString() });
        }
        catch (Exception ex)
        {
            _logService.LogError("Error occurred while changing group machine status.", ex.ToString());
            return StatusCode(500, new { message = "An error occurred while processing the request.", exception = ex.Message });
        }
    }
}
