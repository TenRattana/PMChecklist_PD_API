using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class Users
    {
        [Key]
        [Required]
        public string UserID { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string GUserID { get; set; } = null!;

        [DefaultValue(false)]
        public bool IsActive { get; set; }

        public GroupUsers GroupUser { get; set; } = null!;
    }
}
