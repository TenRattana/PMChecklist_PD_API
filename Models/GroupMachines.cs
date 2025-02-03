using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class GroupMachines
    {
        public string? GMachineID { get; set; }

        [Required(ErrorMessage = "The group machine name is required.")]
        [StringLength(100, ErrorMessage = "The group machine name must be at most 100 characters.")]
        public string? GMachineName { get; set; }

        [Required(ErrorMessage = "The description is required.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The status field is required.")]
        [DefaultValue(false)]
        public bool? IsActive { get; set; }  
        public bool Disables { get; set; }
        public bool Deletes { get; set; }
        public int RowNum { get; set; }
    }
}
