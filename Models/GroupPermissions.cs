using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class GroupPermission
    {
        public string? GuserID { get; set; }
        public int PermissionID { get; set; }
        public bool? PermissionStatus { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }

        public GroupUsers? GroupUser { get; set; }

        public Permissions? Permission { get; set; }
    }
}
