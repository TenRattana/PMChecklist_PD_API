using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class ExpectedResult
    {
        public int EResultID { get; set; }
        public int? MCListID { get; set; }
        public string? FormID { get; set; }
        public string? MachineID { get; set; }
        public string? GCLOptionID { get; set; }
        public string? UserID { get; set; }
        public string? TableID { get; set; }
        public string? EResult { get; set; }
        public string? ApprovedID { get; set; }
        public bool? Status { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
