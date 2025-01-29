using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class Users
    {
        public string? UserID { get; set; }
        public string? UserName { get; set; }
        public string? GUserID { get; set; }
        public string? GUserName { get; set; }
        public bool IsActive { get; set; }

        public GroupUsers? GroupUser { get; set; }
    }
}