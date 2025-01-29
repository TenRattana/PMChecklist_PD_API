using Microsoft.AspNetCore.Mvc;
using PMChecklist_PD_API.Services;
using PMChecklist_PD_API.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class LdapController : ControllerBase
{
    private readonly LdapService _ldapService;
    private readonly Common _common;

    public LdapController(LdapService ldapService, Common common)
    {
        _ldapService = ldapService;
        _common = common;
    }

    [HttpGet("AuthenticateUser")]
    public async Task<ActionResult<List<LdapUser>>> AuthenticateUser(string UserName, string Password)
    {
        var users = await _ldapService.AuthenticateAsync(UserName, Password);

        if (users == null || !users.Any())
        {
            return NotFound("User not found.");
        }
        
        string token = _common.GenerateJwtToken(users.First().UserName!, users.First().Permissions!);

        return Ok(new { token });
    }

}


