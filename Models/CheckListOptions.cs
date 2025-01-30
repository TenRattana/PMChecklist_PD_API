using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class CheckListOptions
    {
        public string? CLOptionID { get; set; }
        public string? CLOptionName { get; set; }
        public bool? IsActive { get; set; }
        public bool Disables { get; set; }
        public int RowNum { get; set; }
    }
}
