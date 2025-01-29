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
    public ActionResult<CheckListTypes> GetCheckListTypes()
    {
        try
        {
            var data = _connection.QueryData<CheckListTypes>("SELECT GTypeID , GTypeName , Icon , IsActive FROM GroupTypeCheckLists", new { });

            var result = new List<object>();

            foreach (var item in data)
            {
                var CheckListType = _connection.QueryData<CheckListTypes>("EXEC GetGroupTypeCheckListsInPage @ID", new { ID = item.GTypeID });

                var resultItem = new
                {
                    item.GTypeID,
                    item.GTypeName,
                    item.Icon,
                    item.IsActive,
                    CheckListType
                };

                result.Add(resultItem);
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
