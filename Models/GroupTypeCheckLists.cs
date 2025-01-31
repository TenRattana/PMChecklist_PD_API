using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class GroupTypeCheckLists
    {
        public string? GTypeID { get; set; }
        public string? GTypeName { get; set; }
        public string? Icon { get; set; }
        public bool? IsActive { get; set; }

        public CheckListTypes? CheckListTypes {get; set;}
    }
}
