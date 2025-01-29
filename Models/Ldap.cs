
namespace PMChecklist_PD_API.Models
{
    public class LdapUser
    {
        public string? UserID { get; set; }
        public string? GUserID { get; set; }
        public string? GUserName { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
        public string[]? Permissons { get; set; }
    }
}
