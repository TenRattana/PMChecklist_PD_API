using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_match_checklist_option")]
public class MatchCheckListOptionController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public MatchCheckListOptionController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet("/GetMatchCheckListOptions/{page}/{pageSize}")]
    public ActionResult<MatchCheckListOption> GetMatchCheckListOptions(int page, int pageSize)
    {
        try
        {
            var data = _connection.QueryData<MatchCheckListOption>("EXEC GetMachinesInPage @PageIndex , @PageSize", new { PageIndex = page, PageSize = pageSize });

            var result = new List<object>();

            foreach (var item in data)
            {
                var CheckListOptions = _connection.QueryData<CheckListOptions>("Select clo.CLOptionID, clo.CLOptionName, clo.IsActive From MatchCheckListOption As mclo INNER JOIN CheckListOptions As clo On mclo.CLOptionID = clo.CLOptionID Where mclo.MCLOptionID In (@MCLOptionID)", new { MCLOptionID = item.MCLOptionID! });

                var resultItem = new
                {
                    item.MCLOptionID,
                    item.CLOptionName,
                    item.GCLOptionName,
                    item.DisplayOrder,
                    item.GCLOptionID,
                    item.IsActive,
                    item.Disables,
                    item.Deletes,
                    item.RowNum,
                    CheckListOptions
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

    [HttpGet("/SearchMatchCheckListOptions/{Messages}")]
    public ActionResult<Machines> SearchMatchCheckListOptions(string Messages)
    {
        try
        {
            var data = _connection.QueryData<MatchCheckListOption>("EXEC SearchMatchGroupCheckListAndOptionWithPagination @SearchTerm", new { SearchTerm = Messages });

            var result = new List<object>();

            foreach (var item in data)
            {
                var CheckListOptions = _connection.QueryData<CheckListOptions>("Select clo.CLOptionID, clo.CLOptionName, clo.IsActive From MatchCheckListOption As mclo INNER JOIN CheckListOptions As clo On mclo.CLOptionID = clo.CLOptionID Where mclo.MCLOptionID In (@MCLOptionID)", new { MCLOptionID = item.MCLOptionID! });

                var resultItem = new
                {
                    item.MCLOptionID,
                    item.CLOptionName,
                    item.GCLOptionName,
                    item.DisplayOrder,
                    item.GCLOptionID,
                    item.IsActive,
                    item.Disables,
                    item.Deletes,
                    item.RowNum,
                    CheckListOptions
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

    [HttpGet("/GetMatchCheckListOption/{MCLOptionID}/{mode}")]
    public ActionResult<Machines> GetMatchCheckListOption(string MCLOptionID, bool mode)
    {
        try
        {
            var data = _connection.QueryData<MatchCheckListOption>("EXEC GetMatchGroupCheckListAndOptionInPage @ID", new { ID = MCLOptionID });

            var result = new List<object>();

            foreach (var item in data)
            {
                var query = "Select clo.CLOptionID, clo.CLOptionName, clo.IsActive From MatchCheckListOption As mclo INNER JOIN CheckListOptions As clo On mclo.CLOptionID = clo.CLOptionID Where mclo.MCLOptionID In (@MCLOptionID) ";
                if (mode)
                {
                    query = string.Concat(query, "AND mclo.IsActive = 'True'");
                }
                var CheckListOptions = _connection.QueryData<CheckListOptions>(query, new { MCLOptionID = item.MCLOptionID! });

                var resultItem = new
                {
                    item.MCLOptionID,
                    item.CLOptionName,
                    item.GCLOptionName,
                    item.DisplayOrder,
                    item.GCLOptionID,
                    item.IsActive,
                    item.Disables,
                    item.Deletes,
                    item.RowNum,
                    CheckListOptions
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
