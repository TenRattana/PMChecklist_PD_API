using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_login")]
public class GroupUsersController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public GroupUsersController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet("/GetGroupUsers")]
    public ActionResult<GroupUsers> GetGroupUsers()
    {

        try
        {
            var data = _connection.QueryData<GroupUsers>("SELECT GUserID, GUserName, IsActive FROM GroupUsers ORDER BY GUserID", new { });

            var result = new List<object>();

            foreach (var item in data)
            {
                var Permissions = _connection.QueryData<Permissions>("SELECT gp.PermissionID, gp.PermissionStatus, gp.IsActive, p.Description as PermissionName From GroupPermissions as gp Inner join Permissions As p On p.PermissionID = gp.PermissionID WHERE gp.GUserID IN (@GuserID) AND gp.IsActive = 1", new { GuserID = item.GUserID! });

                var resultItem = new
                {
                    item.GUserID,
                    item.GUserName,
                    item.IsActive,
                    Permissions
                };

                result.Add(resultItem);
            }

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
