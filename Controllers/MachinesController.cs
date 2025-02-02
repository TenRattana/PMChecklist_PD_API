using System.Text;
using Microsoft.AspNetCore.Mvc;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_machine")]
public class MachinesController : ControllerBase
{
    private readonly Connection _connection;
    private readonly LogService _logService;
    private readonly MachineService _machineService;

    public MachinesController(Connection connection, PCMhecklistContext context, LogService logService, MachineService machineService)
    {
        _connection = connection;
        _logService = logService;
        _machineService = machineService;
    }

    [HttpGet("/GetMachines/{page}/{pageSize}")]
    public ActionResult<Machines> GetMachines(int page, int pageSize)
    {
        try
        {
            var data = _connection.QueryData<Machines>("EXEC GetMachinesInPage @PageIndex , @PageSize", new { PageIndex = page, PageSize = pageSize });

            if (data == null || !data.Any())
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

    [HttpGet("/SearchMachines/{Messages}")]
    public ActionResult<Machines> SearchMachines(string Messages)
    {
        try
        {
            var data = _connection.QueryData<Machines>("EXEC SearchMachinesWithPagination @SearchTerm", new { SearchTerm = Messages });

            if (data == null || !data.Any())
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

    [HttpGet("/GetMachine/{MachineID}")]
    public ActionResult<GroupMachines> GetMachine(string MachineID)
    {
        var errors = new List<string>();

        try
        {
            if (string.IsNullOrWhiteSpace(MachineID)) errors.Add("The machine id field is required.");

            if (errors.Any()) return BadRequest(new { errors });

            var data = _connection.QueryData<GroupMachines>("EXEC GetMachinesInPage @ID = @MachineID", new { MachineID });

            if (data == null || !data.Any())
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

    [HttpPost("SaveMachine")]
    public IActionResult SaveMachine([FromBody] Machines machine)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { errors });
        }

        try
        {
            var exists = _connection.QueryData<dynamic>("SELECT CASE WHEN EXISTS (SELECT 1 FROM Machines WHERE MachineName = @MachineName AND MachineID NOT IN (@MachineID)) THEN 1 ELSE 0 END AS NameCount, CASE WHEN EXISTS (SELECT 1 FROM Machines WHERE MachineCode = @MachineCode AND MachineID NOT IN (@MachineID)) THEN 1 ELSE 0 END AS CodeCount",
            new { MachineID = machine.MachineID ?? "", MachineCode = machine.MachineCode!, MachineName = machine.MachineName! }).FirstOrDefault();

            if (exists != null)
            {
                if (Convert.ToInt32(exists!.NameCount) == 1) return BadRequest(new { message = "The machine name already exists." });
                if (Convert.ToInt32(exists!.CodeCount) == 1) return BadRequest(new { message = "The machine code already exists." });
            }

            var data = _connection.QueryData<dynamic>("EXEC GetMachinesInPage @ID = @MachineID", new { MachineID = machine.MachineID ?? "" }).FirstOrDefault();

            if (data != null && Convert.ToInt32(data!.NameCount) == 1)
            {
                var Status = Convert.ToBoolean(data!.IsActive);

                if (Status != machine.IsActive) return BadRequest(new { message = "Change ststus not successful.." });
            }

            return Ok(new
            {
                machine.GMachineID,
                machine.MachineName,
                machine.MachineCode,
                machine.IsActive
            });
        }
        catch (Exception ex)
        {
            _logService.LogError("Error occurred while changing group machine status.", ex.ToString());
            return StatusCode(500, new { message = "An error occurred while processing the request.", exception = ex.Message });
        }

    }

    [HttpPost("ChangeMachine/{MachineID}")]
    public IActionResult ChangeMachine(string MachineID)
    {
        var errors = new List<string>();
        var logs = new StringBuilder();
        bool status = false;

        try
        {
            if (string.IsNullOrWhiteSpace(MachineID)) errors.Add("The machine id field is required.");

            var data = _connection.QueryData<Machines>("EXEC GetMachinesInPage @ID = @MachineID", new { MachineID }).FirstOrDefault();

            if (data == null) errors.Add("The machine id field does not exist in the database.");

            else
            {
                var disable = Convert.ToBoolean(data.Disables);
                if (!disable)
                {
                    status = Convert.ToBoolean(data.IsActive);
                    logs.AppendLine("Change Status");
                    logs.AppendLine("----------------------------------------------------");

                    string[] propertiesToLog = { "MachineID", "MachineName", "IsActive" };
                    _logService.AppendObjectPropertiesToLog(ref logs, data, propertiesToLog);
                }
                else errors.Add("Change status not successful.");
            }

            if (errors.Any()) return BadRequest(new { errors });

            var result = _machineService.ChangeMachine(MachineID, status, logs);

            if (result.Any(r => !r.Item2)) return StatusCode(500, new { message = "Failed to change the status.", logs = logs.ToString() });

            return Ok(new { message = "Status changed successfully.", logs = logs.ToString() });
        }
        catch (Exception ex)
        {
            _logService.LogError("Error occurred while changing group machine status.", ex.ToString());
            return StatusCode(500, new { message = "An error occurred while processing the request.", exception = ex.Message });
        }
    }

    [HttpDelete("DeleteMachine/{MachineID}")]
    public IActionResult DeleteMachine(string MachineID)
    {
        var errors = new List<string>();
        var logs = new StringBuilder();

        try
        {
            if (string.IsNullOrWhiteSpace(MachineID)) errors.Add("The machine id field is required.");

            var data = _connection.QueryData<Machines>("EXEC GetMachinesInPage @ID = @GMachineID", new { MachineID }).FirstOrDefault();

            if (data == null) errors.Add("The machine id field does not exist in the database.");

            else
            {
                var delete = Convert.ToBoolean(data.Deletes);
                if (!delete)
                {
                    logs.AppendLine("Delete Data");
                    logs.AppendLine("----------------------------------------------------");

                    string[] propertiesToLog = { "MachineID", "MachineName" };
                    _logService.AppendObjectPropertiesToLog(ref logs, data, propertiesToLog);
                }
                else errors.Add("Delete not successful.");
            }

            if (errors.Any()) return BadRequest(new { errors });

            var result = _machineService.DeleteMachine(MachineID, logs);

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
