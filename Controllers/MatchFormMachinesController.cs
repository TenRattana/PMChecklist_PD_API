using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_checklist")]
public class MatchFormMachinesController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public MatchFormMachinesController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet("GetMatchFormMachines/{page}/{pageSize}")]
    public ActionResult<MatchFormMachine> GetMatchFormMachines(int page, int pageSize)
    {
        try
        {
            var data = _connection.QueryData<MatchFormMachine>("EXEC GetMatchFormMachinesInPage @PageIndex , @PageSize", new { PageIndex = page, PageSize = pageSize });

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

    [HttpGet("SearchMatchFormMachine/{Messages}")]
    public ActionResult<MatchFormMachine> SearchMatchFormMachine(string Messages)
    {
        try
        {
            var data = _connection.QueryData<MatchFormMachine>("EXEC SearchMatchFormMachineWithPagination @SearchTerm", new { SearchTerm = Messages });

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

    [HttpGet("GetMatchFormMachine/{MachineID}")]
    public ActionResult<MatchFormMachine> GetMatchFormMachine(string MachineID)
    {
        try
        {
            var data = _connection.QueryData<MatchFormMachine>("EXEC GetMatchFormMachinesInPage @ID = @MachineID", new { MachineID });

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
