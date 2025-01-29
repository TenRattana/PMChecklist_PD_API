using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class CheckListTypes
    {
        public string? CTypeID { get; set; }
        public string? GTypeID { get; set; }
        public string? CTypeName { get; set; }
        public string? GTypeName { get; set; }
        public byte? Displayorder { get; set; }
        public string? Icon { get; set; }
        public bool? IsActive { get; set; }

        public CheckListTypes? CheckListType { get; set; }

        public GroupTypeCheckLists? GroupTypeCheckLists { get; set; }
    }
}
