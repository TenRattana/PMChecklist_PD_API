using Microsoft.AspNetCore.Mvc;
using PMChecklist_PD_API.Services;
using PMChecklist_PD_API.Models;

public class LdapController : ControllerBase
{
    private readonly LdapService _ldapService;

    public LdapController(LdapService ldapService)
    {
        _ldapService = ldapService;
    }

    [HttpGet("AuthenticateUser")]
    public async Task<ActionResult<List<LdapUser>>> AuthenticateUser(string username, string password)
    {
        var users = await _ldapService.AuthenticateAsync(username, password);

        if (users == null || !users.Any())
        {
            return NotFound("User not found.");
        }

        return Ok(users);
    }

}


