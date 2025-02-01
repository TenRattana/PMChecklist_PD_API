using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_machine")]
public class MachinesController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public MachinesController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
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
            Console.WriteLine(ex);
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
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

    [HttpGet("/GetMachine/{MachineID}")]
    public ActionResult<GroupMachines> GetMachine(string MachineID)
    {
        try
        {
            var data = _connection.QueryData<GroupMachines>("EXEC GetMachinesInPage @ID = @MachineID", new { MachineID });

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
