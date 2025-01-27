using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class TimeSlots
    {
        public int TSlotID { get; set; }
        public string? ScheduleID { get; set; }
        public string? Day { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }

        public TimeSchedules? TimeSchedule { get; set; }
    }
}
