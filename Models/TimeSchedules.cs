using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class TimeSchedules
    {
        public string? ScheduleID { get; set; }
        public string? ScheduleName { get; set; }
        public string? TypeSchedule { get; set; }
        public bool? Tracking { get; set; }
        public bool? Custom { get; set; }
        public bool? IsActive { get; set; }
    }
}
