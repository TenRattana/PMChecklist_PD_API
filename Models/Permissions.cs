using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class Permissions
    {
        public int PermissionID { get; set; } 
        public string? PermissionName { get; set; }  
        public string? Description { get; set; } 
        public bool? IsActive { get; set; } 
    }
}
