using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class Menu
    {
        public int MenuID { get; set; }
        public string? MenuLabel { get; set; }
        public string? MenuPermission { get; set; }
        public string? PermissionStatus { get; set; }
        public string? NavigationTo { get; set; }
        public int OrderNo { get; set; }
        public int ParentMenuID { get; set; }
        public string? Path { get; set; }
        public string? Permission { get; set; }
        public int PermissionID { get; set; }
        public string? Description { get; set; }
    }
}
