using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class MatchFormMachine
    {
       public string? MachineID { get; set; } 
        public string? FormID { get; set; }  
        public string? GMachineID { get; set; }  
        public string? FormName { get; set; }  
        public string? MachineCode { get; set; }
        public string? MachineName { get; set; }
        public string? GMachineName { get; set; }
        public string? Building { get; set; }
        public string? Floor { get; set; }
        public string? Area { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; } 
        public bool? IsActiveForm { get; set; } 
        public bool? Disables { get; set; } 
        public int RowNum { get; set; }
    }
}
