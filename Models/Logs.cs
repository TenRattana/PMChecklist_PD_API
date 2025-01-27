using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }  = null!;
        [Required]
        public string Author { get; set; } = null!;
        [Required]
        public string Messages { get; set; } = null!;
        [Required]
        public string Type { get; set; } = null!;
        public DateTime Create_Date { get; set; }
    }
}
