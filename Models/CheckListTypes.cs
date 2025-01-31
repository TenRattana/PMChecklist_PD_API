using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class CheckListTypes
    {
        public string? CTypeID { get; set; }
        public string? CTypeName { get; set; }
        public string? CTypeTitle { get; set; }
        public string? Icon { get; set; }
        public bool IsActive { get; set; }
        public int RowNum { get; set; }
    }
}
