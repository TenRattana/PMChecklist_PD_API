using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class GroupUsers
    {
        [Key]
        [Required]
        public string GUserID { get; set; } = null!;

        [Required]
        public string GUserName { get; set; } = null!;

        [DefaultValue(false)]
        public bool IsActive { get; set; }
    }
}
