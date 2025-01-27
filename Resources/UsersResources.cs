namespace PMChecklist_PD_API.Resources
{
    public class UsersResources
    {
        public string UserID { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string GUserID { get; set; } = null!;
        public string GUserName { get; set; } = null!;
        public bool IsActive { get; set; } = false!;
    }
}
