using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class CheckListOptions
    {
        public string? CLOptionID { get; set; }

        [Required(ErrorMessage = "The check list option name is required.")]
        [StringLength(100, ErrorMessage = "The check list option name must be at most 100 characters.")]
        public string? CLOptionName { get; set; }

        [Required(ErrorMessage = "The status field is required.")]
        public bool? IsActive { get; set; }
        public bool Disables { get; set; }
        public int RowNum { get; set; }
    }
}
