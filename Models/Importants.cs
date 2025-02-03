using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class Importants
    {
        public int ImportantID { get; set; }
        public int? MCListID { get; set; }
        public string? Value { get; set; }
        public float? MinLength { get; set; }
        public float? MaxLength { get; set; }
        public MatchCheckList? MatchCheckList { get; set; }
    }
}
