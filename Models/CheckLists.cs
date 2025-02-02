using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class CheckLists
    {
        public string? CListID { get; set; }

        [Required(ErrorMessage = "The check list name is required.")]
        [StringLength(100, ErrorMessage = "The check list name must be at most 100 characters.")]
        public string? CListName { get; set; }
        
        [Required(ErrorMessage = "The status field is required.")]
        public bool? IsActive { get; set; }
        public bool Disables { get; set; }
        public int RowNum { get; set; }
    }
}
