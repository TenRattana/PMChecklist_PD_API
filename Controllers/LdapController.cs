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
    public async Task<ActionResult<List<LdapUser>>> AuthenticateUser(string username, string password)
    {
        var users = await _ldapService.AuthenticateAsync(username, password);

        if (users == null || !users.Any())
        {
            return NotFound("User not found.");
        }

        var tokens = _common.GenerateJwtToken(username , users.First().GUserName!);

        return Ok(new { tokens });
    }

}


