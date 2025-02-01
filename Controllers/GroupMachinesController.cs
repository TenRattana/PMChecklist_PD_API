using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

            if (data == null || !data.Any())
            {
                return NotFound(new { status = false, message = "No data found." });
            }

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

            if (data == null || !data.Any())
            {
                return NotFound(new { status = false, message = "No data found." });
            }

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
            if (string.IsNullOrWhiteSpace(GMachineID))
            {
                errors.Add("The group machine id field is required.");
            }

            if (errors.Any())
            {
                return BadRequest(new { errors });
            }

            var data = _connection.QueryData<GroupMachines>("EXEC GetGroupMachinesInPage @ID = @GMachineID", new { GMachineID }).FirstOrDefault();

            if (data == null)
            {
                return NotFound(new { status = false, message = "No data found." });
            }

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logService.LogError("Error occurred while fetching group machine.", ex.ToString());
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
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
            if (string.IsNullOrWhiteSpace(GMachineID))
            {
                errors.Add("The group machine id field is required.");
            }

            var data = _connection.QueryData<GroupMachines>("EXEC GetGroupMachinesInPage @ID = @GMachineID", new { GMachineID }).FirstOrDefault();

            if (data == null)
            {
                errors.Add("The group machine id field does not exist in the database.");
            }

            else
            {
                var disable = Convert.ToBoolean(data.Disables);
                if (!disable)
                {
                    status = Convert.ToBoolean(data.IsActive);
                    logs.AppendLine("Change Status");
                    logs.AppendLine("----------------------------------------------------");
                    logs.AppendLine($"Group machine id: {data.GMachineID}");
                    logs.AppendLine($"Group machine name: {data.GMachineName}");
                    logs.AppendLine($"IsActive: {status}");
                }
                else
                {
                    errors.Add("Change status not successful.");
                }
            }

            if (errors.Any())
            {
                return BadRequest(new { errors });
            }

            var result = _groupMachineService.ChangeGroupMachine(GMachineID, status, logs);

            if (result.Any(r => !r.Item2))
            {
                return StatusCode(500, new { message = "Failed to change the status.", logs = logs.ToString() });
            }

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
            if (string.IsNullOrWhiteSpace(GMachineID))
            {
                errors.Add("The group machine id field is required.");
            }

            var data = _connection.QueryData<GroupMachines>("EXEC GetGroupMachinesInPage @ID = @GMachineID", new { GMachineID }).FirstOrDefault();

            if (data == null)
            {
                errors.Add("The group machine id field does not exist in the database.");
            }
            else
            {
                var delete = Convert.ToBoolean(data.Deletes);
                if (!delete)
                {
                    logs.AppendLine("Delete Data");
                    logs.AppendLine("----------------------------------------------------");
                    logs.AppendLine($"Group machine id : {data.GMachineID}");
                    logs.AppendLine($"Group machine name : {data.GMachineName}");
                }
                else
                {
                    errors.Add("Delete not successful.");
                }
            }

            if (errors.Any())
            {
                return BadRequest(new { errors });
            }

            var result = _groupMachineService.DeleteGroupMachine(GMachineID, logs);

            if (result.Any(r => !r.Item2))
            {
                return StatusCode(500, new { message = "Failed to change the status.", logs = logs.ToString() });
            }

            return Ok(new { message = "Status changed successfully.", logs = logs.ToString() });
        }
        catch (Exception ex)
        {
            _logService.LogError("Error occurred while changing group machine status.", ex.ToString());
            return StatusCode(500, new { message = "An error occurred while processing the request.", exception = ex.Message });
        }
    }
}
