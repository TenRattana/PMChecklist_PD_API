using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class SubForms
    {
        public string? SFormID { get; set; }
        public string? FormID { get; set; }
        public string? SFormName { get; set; }
        public byte Columns { get; set; }
        public byte DisplayOrder { get; set; }
    }
}
