using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class FiscalYears
    {
        [Key]
        [Required]
        public int FiscalID { get; set; }
        [Required]
        public string FiscalYear { get; set; } = null!;
    }
}
