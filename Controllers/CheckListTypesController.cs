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
public class CheckListTypesController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public CheckListTypesController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet("/GetCheckListTypes")]
    public ActionResult<GroupTypeCheckLists> GetCheckListTypes()
    {
        try
        {
            var data = _connection.QueryData<GroupTypeCheckLists>("SELECT GTypeID , GTypeName , Icon , IsActive FROM GroupTypeCheckLists", new { });

            if (data == null || !data.Any())
            {
                return NotFound(new { status = false, message = "No data found." });
            }

            var result = new List<object>();

            foreach (var item in data)
            {
                var GTypeID = item.GTypeID;
                var CheckListTypes = _connection.QueryData<CheckListTypes>("EXEC GetGroupTypeCheckListsInPage @ID", new { ID = GTypeID });

                var resultItem = new
                {
                    item.GTypeID,
                    item.GTypeName,
                    item.Icon,
                    item.IsActive,
                    CheckListTypes
                };

                result.Add(resultItem);
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
