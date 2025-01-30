using Microsoft.AspNetCore.Mvc;
using PMChecklist_PD_API.Models;

[ApiController]
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

    /// <summary>
    /// Authenticate User and return a JWT token.
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="Password"></param>
    /// <returns></returns>
    [HttpGet("AuthenticateUser")]
    public ActionResult<List<object>> AuthenticateUser(string UserName, string Password)
    {
        var users =  _ldapService.AuthenticateAsync(UserName, Password);

        if (users == null)
        {
            return NotFound("User not found.");
        }
        
        string token = _common.GenerateJwtToken(users);

        return Ok(new { token });
    }

}


