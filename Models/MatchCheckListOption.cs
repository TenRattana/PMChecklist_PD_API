using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class MatchCheckListOption
    {
        public string? MCLOptionID { get; set; }
        public string? GCLOptionID { get; set; }
        public string? CLOptionID { get; set; }
        public byte? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }

        public GroupCheckListOptions? GroupCheckListOption { get; set; }
        public CheckListOptions? CheckListOption { get; set; }
    }
}
