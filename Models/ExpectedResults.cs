using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class ExpectedResult
    {
        public string? MachineName { get; set; }
        public int? MCListID { get; set; }
        public string? FormID { get; set; }
        public string? FormName { get; set; }
        public string? MachineID { get; set; }
        public string? GCLOptionID { get; set; }
        public string? UserID { get; set; }
        public string? TableID { get; set; }
        public string? ApprovedID { get; set; }
        public string? UserName { get; set; }
        public bool? Status { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public DateTime CreateDate { get; set; }
        public int RowNum { get; set; }

    }
}
