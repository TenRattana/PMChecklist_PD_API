using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_login", "view_home")]
public class AppConfigController : ControllerBase
{
    private readonly ILogger<AppConfigController> _logger;
    private readonly PCMhecklistContext _context;

    public AppConfigController(ILogger<AppConfigController> logger, PCMhecklistContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("/GetAppConfig")]
    public async Task<ActionResult> GetAppConfig()
    {
        try
        {
            _logger.LogInformation("Fetching AppConfig data.");

            var data = await _context.AppConfig.ToListAsync();
            return Ok(new { status = true, message = "Select success", data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching app config data.");
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
            _logger.LogError(ex, "Error occurred while saving app config data.");
            return StatusCode(500, new { status = false, message = "An error occurred while saving the data. Please try again later." });
        }
    }
}

