using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
public class Ldap
{
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
}
