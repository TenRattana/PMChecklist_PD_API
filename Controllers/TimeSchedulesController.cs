using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[CustomRoleAuthorize("view_login")]
public class TimeSchedulesController : ControllerBase
{
    private readonly Connection _connection;
    private readonly PCMhecklistContext _context;
    private readonly TimeSchedulesService _timeschedule;

    public TimeSchedulesController(Connection connection, PCMhecklistContext context, TimeSchedulesService timeschedules)
    {
        _connection = connection;
        _context = context;
        _timeschedule = timeschedules;
    }

    [HttpPost("GetSchedules")]
    public ActionResult<TimeSchedules> GetSchedules()
    {
        try
        {
            var data = _connection.QueryData<TimeSchedules>("SELECT ts.ScheduleID, ts.ScheduleName, ts.Type_schedule, ts.IsActive, ts.Tracking, ts.Custom FROM TimeSchedules AS ts ORDER BY ts.ScheduleID DESC", new { });

            if (data == null || !data.Any())
            {
                return NotFound(new { status = false, message = "No data found." });
            }

            var result = new List<object>();

            foreach (var item in data)
            {
                var ScheduleID = item.ScheduleID!;
                var TypeSchedule = item.Type_schedule!;
                var ScheduleName = item.ScheduleName!;
                var IsActive = item.IsActive!;

                var schedule = _timeschedule.BodyTime(item);

                var resultItem = new
                {
                    ScheduleID,
                    ScheduleName = item.ScheduleName!,
                    Type_schedule = TypeSchedule!,
                    IsActive = item.IsActive!,
                    Tracking = item.Tracking!,
                    Custom = item.Custom!,
                    MachineGroup = schedule["MachineGroup"],
                    TimelineItems = schedule["TimelineItems"]
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
