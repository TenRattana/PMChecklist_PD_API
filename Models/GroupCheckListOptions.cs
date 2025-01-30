using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class GroupCheckListOptions
    {
        public string? GCLOptionID { get; set; }
        public string? GCLOptionName { get; set; }
        public bool IsActive { get; set; }
        public bool Disables { get; set; }
        public bool Deletes { get; set; }
        public int RowNum { get; set; }
    }
}
