using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using PMChecklist_PD_API.Models;

public class TimeSchedulesService
{
    private readonly Connection _connection;
    private readonly LogService _logger;
    private readonly Common _common;

    public TimeSchedulesService(Connection connection, LogService logger, Common common)
    {
        _common = common;
        _connection = connection;
        _logger = logger;
    }

    public Dictionary<string, object> BodyTime(TimeSchedules schedules)
    {
        var bodyGroupMachine = _connection.QueryData<GroupMachines>("EXEC SearchGroupMachinesWithPagination @SearchTerm = @ScheduleID", new { ScheduleID = schedules.ScheduleID! });

        var machineGroup = new List<object>();

        foreach (var item in bodyGroupMachine)
        {
            var resultItem = new
            {
                GMachineID = item.GMachineID!,
                GMachineName = item.GMachineName!
            };

            machineGroup.Add(resultItem);
        }

        var bodyType = _connection.QueryData<dynamic>("EXEC GetScheduleData @ScheduleID = @ScheduleID , @TypeSchedule = @TypeSchedule", new { ScheduleID = schedules.ScheduleID!, TypeSchedule = schedules.Type_schedule! });

        var data = new Dictionary<string, object>
        {
            { "MachineGroup", machineGroup },
            { "TimelineItems", BodyType(bodyType , schedules) }
        };

        return data;
    }

    private List<object> BodyType(IEnumerable<dynamic>? bodyType, TimeSchedules schedules)
    {

        var timelineItems = new List<object>();

        switch (schedules.Type_schedule)
        {
            case "Daily":
                foreach (var item in bodyType!)
                {
                    var resultItem = new
                    {
                        ScheduleID = schedules.ScheduleID!,
                        date = "Recurring Daily",
                        name = schedules.ScheduleName,
                        status = schedules.IsActive,
                        time = FormatTimeRange(item.start, item.end),
                    };

                    timelineItems.Add(resultItem);
                }
                break;

            case "Weekly":
                foreach (var item in bodyType!)
                {
                    var resultItem = new
                    {
                        ScheduleID = schedules.ScheduleID!,
                        date = item.Day?.ToString()!,
                        name = schedules.ScheduleName,
                        status = schedules.IsActive,
                        time = FormatTimeRange(item.start, item.end),
                    };

                    timelineItems.Add(resultItem);
                }
                break;

            case "Custom":
                foreach (var item in bodyType!)
                {
                    var start = item.start?.ToString();
                    var end = item.end?.ToString();

                    if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
                    {
                        var startDate = ExtractDate(start);
                        var startTime = ExtractTime(start);
                        var endTime = ExtractTime(end);

                        var resultItem = new
                        {
                            ScheduleID = schedules.ScheduleID!,
                            date = $"Custom ({startDate})",
                            name = schedules.ScheduleName,
                            status = schedules.IsActive,
                            time = $"{startTime} - {endTime}",
                        };

                        timelineItems.Add(resultItem);
                    }
                }
                break;

            default:
                break;
        }

        return timelineItems;
    }

    private string FormatTimeRange(string start, string end)
    {
        var startTime = ExtractTime(start);
        var endTime = ExtractTime(end);
        return $"{startTime} - {endTime}";
    }

    private string ExtractDate(string dateTime)
    {
        if (DateTime.TryParse(dateTime, out var parsedDate))
        {
            return parsedDate.ToString("yyyy-MM-dd");
        }
        return "Invalid Date";
    }

    private string ExtractTime(string dateTime)
    {
        if (DateTime.TryParse(dateTime, out var parsedDate))
        {
            return parsedDate.ToString("HH:mm");
        }
        return "00:00";
    }
}
