using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class CheckLists
    {
        public string? CListID { get; set; }
        public string? CListName { get; set; }
        public bool? IsActive { get; set; }
    }
}
