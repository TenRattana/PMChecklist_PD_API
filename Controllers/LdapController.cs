using Microsoft.AspNetCore.Mvc;
using PMChecklist_PD_API.Models;

public class LdapController : ControllerBase
{
    private readonly LdapService _ldapService;
    private readonly Common _common;

    public LdapController(LdapService ldapService, Common common)
    {
        _ldapService = ldapService;
        _common = common;
    }

    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromForm] Ldap login)
    {
        bool isAuthenticated = _ldapService.AuthenticateUser(login.Username, login.Password);
        if (isAuthenticated)
        {
            var token = _common.GenerateJwtToken(login.Username);
            return Ok(new { Token = token });
        }
        else
        {
            return Unauthorized("Invalid credentials");
        }
    }

    [HttpGet("user-info")]
    public IActionResult GetUserInfo([FromQuery] string username)
    {
        string userInfo = _ldapService.GetUserInfo(username);
        return Ok(userInfo);
    }
}
