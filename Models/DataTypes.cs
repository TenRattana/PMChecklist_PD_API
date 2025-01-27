using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class DataTypes
    {
        public string? DTypeID { get; set; }
        public string? DTypeName { get; set; }
        public string? Icon { get; set; }
        public bool? IsActive { get; set; }
    }
}
