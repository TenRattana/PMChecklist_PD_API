using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_approved")]
public class ApprovedsController : ControllerBase
{
    private readonly Connection _connection;

    public ApprovedsController(Connection connection)
    {
        _connection = connection;
    }


    [HttpGet("/GetApproveds/{page}/{pageSize}")]
    public IActionResult GetApproveds(int page, int pageSize)
    {
        try
        {
            var data = _connection.QueryData<Menu>("SELECT TOP (1) * FROM AppConfig", new { });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
