using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class GroupUsers
    {
        public string? GUserID { get; set; }
        public string? GUserName { get; set; }
        [Required(ErrorMessage = "The status field is required.")]
        public bool? IsActive { get; set; }

        public Permissions? Permissions { get; set; }
    }
}
