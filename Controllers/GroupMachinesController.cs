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
    private readonly PCMhecklistContext _context;

    public GroupMachinesController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet("/GetGroupMachines/{page}/{pageSize}")]
    public ActionResult<GroupMachines> GetGroupMachines(int page, int pageSize)
    {
        try
        {
            var data = _connection.QueryData<GroupMachines>("EXEC GetGroupMachinesInPage @PageIndex , @PageSize", new { PageIndex = page, PageSize = pageSize });

            if (data == null || !data.Any())
            {
                return NotFound(new { status = false, message = "No data found." });
            }

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/SearchGroupMachines/{Messages}")]
    public ActionResult<GroupMachines> SearchGroupMachines(string Messages)
    {
        try
        {
            var data = _connection.QueryData<GroupMachines>("EXEC SearchGroupMachinesWithPagination @SearchTerm", new { SearchTerm = Messages });

            if (data == null || !data.Any())
            {
                return NotFound(new { status = false, message = "No data found." });
            }

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetGroupMachine/{GMachineID}")]
    public ActionResult<GroupMachines> GetGroupMachine(string GMachineID)
    {
        try
        {
            var data = _connection.QueryData<GroupMachines>("EXEC GetGroupMachinesInPage @ID", new { ID = GMachineID });

            if (data == null || !data.Any())
            {
                return NotFound(new { status = false, message = "No data found." });
            }

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
