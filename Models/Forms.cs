using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class Forms
    {
        public string? FormID { get; set; }
        public string? FormNumber { get; set; }
        public string? FormName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
