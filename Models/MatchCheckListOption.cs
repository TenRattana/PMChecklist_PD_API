using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class MatchCheckListOption
    {
        public string? MCLOptionID { get; set; }
        public string? GCLOptionID { get; set; }
        public string? CLOptionID { get; set; }
        public string? GCLOptionName { get; set; }
        public string? CLOptionName { get; set; }
        public byte? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public bool? Disables { get; set; }
        public bool? Deletes { get; set; }
        public int RowNum { get; set; }
        public CheckListOptions? CheckListOptions {get; set;} 

    }
}
