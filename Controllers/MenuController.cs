using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_login")]
public class MenuController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public MenuController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpPost("/GetMenus")]
    public ActionResult<object> GetMenus(string GUserID)
    {
        try
        {
            var data = _connection.QueryData<Menu>("EXEC GetMenuPermission @GUserID", new { GUserID });

            var result = new List<object>();

            foreach (var item in data)
            {
                var parentMenuData = _connection.QueryData<Menu>("EXEC GetMenuPermission @GUserID, @ParentMenuID", new { GUserID, ParentMenuID = item.PermissionID });

                var resultItem = new
                {
                    item.MenuID,
                    item.MenuLabel,
                    item.MenuPermission,
                    item.PermissionStatus,
                    item.NavigationTo,
                    item.OrderNo,
                    item.ParentMenuID,
                    item.Path,
                    item.Permission,
                    item.PermissionID,
                    item.Description,
                    ParentMenu = parentMenuData
                };

                result.Add(resultItem);
            }

            if (data == null || !data.Any())
            {
                return NotFound(new { status = false, message = "No data found." });
            }

            return Ok(new { status = true, message = "Select success", data = result });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }
}

