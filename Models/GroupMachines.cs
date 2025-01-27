using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class GroupMachines
    {
        public string? GMachineID { get; set; }
        public string? GMachineName { get; set; }
        public string? Description { get; set; }
        public string? ScheduleID { get; set; }
        public bool IsActive { get; set; }

        public TimeSchedules? TimeSchedule { get; set; }
    }
}
