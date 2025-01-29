using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_login")]
public class DataTypesController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public DataTypesController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet("/GetDataTypes")]
    public ActionResult<DataTypes> GetDataTypes()
    {
        try
        {
            var data = _connection.QueryData<DataTypes>("SELECT DTypeID, DTypeName, Icon, IsActive FROM DataTypes ORDER BY DTypeID", new { });

            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}
