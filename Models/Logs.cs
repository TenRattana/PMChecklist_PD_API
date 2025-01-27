using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }  = null!;
        public string Author { get; set; } = null!;
        public string Messages { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime Create_Date { get; set; }
    }
}
