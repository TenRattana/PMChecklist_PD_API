using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_login")]
public class MenuController : ControllerBase
{
    private readonly ILogger<MenuController> _logger;
    private readonly PCMhecklistContext _context;

    public MenuController(ILogger<MenuController> logger, PCMhecklistContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("/GetMenus")]
    public ActionResult<object> GetMenus(string GUserID)
    {
        try
        {
            var data = _context.Menu.FromSqlRaw("EXEC GetMenuPermission @GUserID = {0}", GUserID).ToList();

            var result = new List<object>();

            foreach (var item in data)
            {
                var parentMenuQuery = "EXEC GetMenuPermission @ParentMenuID = {0}, @GUserID = {1}";
                var parentMenuData = _context.Menu.FromSqlRaw(parentMenuQuery, item.MenuID, GUserID).ToList();

                var resultItem = new
                {
                    ...item,
                    ParentMenu = parentMenuData
                };

                result.Add(resultItem);
            }

            return Ok(new { status = true, message = "Select success", data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while fetching the data. Please try again later." });
        }
    }

}
