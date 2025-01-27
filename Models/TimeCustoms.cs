using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class TimeCustoms
    {
        public int TCustomID { get; set; }
        public string? ScheduleID { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
    }
}
