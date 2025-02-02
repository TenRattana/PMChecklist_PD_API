using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class GroupCheckListOptions
    {
        public string? GCLOptionID { get; set; }

        [Required(ErrorMessage = "The group check list name is required.")]
        [StringLength(100, ErrorMessage = "The group check list name must be at most 100 characters.")]
        public string? GCLOptionName { get; set; }
        
        [Required(ErrorMessage = "The status field is required.")]
        public bool IsActive { get; set; }
        public bool Disables { get; set; }
        public bool Deletes { get; set; }
        public int RowNum { get; set; }
    }
}
