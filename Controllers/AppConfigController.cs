using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_login", "view_home")]
public class AppConfigController : ControllerBase
{

    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;

    public AppConfigController(Connection connection, PCMhecklistContext context)
    {
        _connection = connection;
        _context = context;
    }

    [HttpGet("/GetAppConfig")]
    public ActionResult GetAppConfig()
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

    [HttpPost("/SaveAppConfig")]
    public async Task<ActionResult> SaveAppConfig([FromBody] AppConfig value)
    {
        try
        {
            var appConfig = new AppConfig
            {
                AppID = value.AppID,
                AppName = value.AppName,
                GroupMachine = value.GroupMachine,
                PF_GroupMachine = value.PF_GroupMachine,
                Machine = value.Machine,
                PF_Machine = value.PF_Machine,
                CheckList = value.CheckList,
                PF_CheckList = value.PF_CheckList,
                GroupCheckList = value.GroupCheckList,
                PF_GroupCheckList = value.PF_GroupCheckList,
                CheckListOption = value.CheckListOption,
                PF_CheckListOption = value.PF_CheckListOption,
                MatchCheckListOption = value.MatchCheckListOption,
                PF_MatchCheckListOption = value.PF_MatchCheckListOption,
                MatchFormMachine = value.MatchFormMachine,
                PF_MatchFormMachine = value.PF_MatchFormMachine,
                Form = value.Form,
                PF_Form = value.PF_Form,
                SubForm = value.SubForm,
                PF_SubForm = value.PF_SubForm,
                ExpectedResult = value.ExpectedResult,
                PF_ExpectedResult = value.PF_ExpectedResult,
                UsersPermission = value.UsersPermission,
                PF_UsersPermission = value.PF_UsersPermission,
                TimeSchedule = value.TimeSchedule,
                PF_TimeSchedule = value.PF_TimeSchedule
            };

            _context.AppConfig.Add(appConfig);
            await _context.SaveChangesAsync();
            return Ok(new { status = true, message = "AppConfig created successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { status = false, message = "An error occurred while saving the data. Please try again later." });
        }
    }
}

