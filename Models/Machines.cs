using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class Machines
    {
        public string? MachineID { get; set; }
        public string? FormID { get; set; }
        public string? GMachineID { get; set; }

        [Required(ErrorMessage = "The machine code is required.")]
        [StringLength(100, ErrorMessage = "The machine code must be at most 100 characters.")]
        public string? MachineCode { get; set; }

        [Required(ErrorMessage = "The machine name is required.")]
        [StringLength(100, ErrorMessage = "The machine name must be at most 100 characters.")]
        public string? MachineName { get; set; }
        public string? GMachineName { get; set; }

        [StringLength(50, ErrorMessage = "The machine name must be at most 50 characters.")]
        public string? Building { get; set; }

        [StringLength(50, ErrorMessage = "The machine name must be at most 50 characters.")]
        public string? Floor { get; set; }

        [StringLength(50, ErrorMessage = "The machine name must be at most 50 characters.")]
        public string? Area { get; set; }

        [Required(ErrorMessage = "The description is required.")]
        [StringLength(100, ErrorMessage = "The description must be at most 100 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The status field is required.")]
        [DefaultValue(false)]
        public bool? IsActive { get; set; }
        public bool? Disables { get; set; }
        public bool? Deletes { get; set; }
        public int RowNum { get; set; }
    }
}
