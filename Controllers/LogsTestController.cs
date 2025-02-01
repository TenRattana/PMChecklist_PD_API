using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_approved")]
public class LogsTestController : ControllerBase
{
    private readonly LogService _logs;
    private readonly ILogger<LogsTestController> _logger;

    public LogsTestController(LogService logs, ILogger<LogsTestController> logger)
    {
        _logs = logs;
        _logger = logger;
    }

    /// <summary>
    /// Select a data Approved from database with page and pagesize.
    /// </summary>
    [HttpPost("/LogsInfo")]
    public ActionResult<string> LogsInfo([FromBody] Log value)
    {
        try
        {
            if (value == null)
            {
                _logger.LogWarning("LogsInfo called with null value.");
                return BadRequest("Log value cannot be null.");
            }

            _logs.LogInfo(value);
            return Ok("LogInfo Inserted Successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred in LogsInfo: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred while processing the log.", error = ex.Message });
        }
    }
}
