using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class TimeSchedules
    {
        public string? ScheduleID { get; set; }
        [Required(ErrorMessage = "The time schedule name is required.")]
        [StringLength(100, ErrorMessage = "The time schedule name must be at most 100 characters.")]
        public string? ScheduleName { get; set; }
        public string? Type_schedule { get; set; }
        [Required(ErrorMessage = "The track field is required.")]
        [DefaultValue(false)]
        public bool? Tracking { get; set; }
        [Required(ErrorMessage = "The custom field is required.")]
        [DefaultValue(false)]
        public bool? Custom { get; set; }
        [Required(ErrorMessage = "The status field is required.")]
        [DefaultValue(false)]
        public bool? IsActive { get; set; }
    }
}
