using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class TimeTracks
    {
        public int TTrackID { get; set; }
        public string? ScheduleID { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }

        public TimeSchedules? TimeSchedule { get; set; }
    }
}
