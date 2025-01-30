using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class GroupMachines
    {
        public string? GMachineID { get; set; }
        public string? GMachineName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool Disables { get; set; }
        public int RowNum { get; set; }
    }
}
