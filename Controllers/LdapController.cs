using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    /// <summary>
    /// Authenticate User and return a JWT token.
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="Password"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("AuthenticateUser")]
    public ActionResult<List<object>> AuthenticateUser(string UserName, string Password)
    {
        var users = _ldapService.AuthenticateAsync(UserName, Password);

        if (users == null)
        {
            return Unauthorized();
        }

        string token = _common.GenerateJwtToken(users);

        return Ok(new { token });
    }

    [AllowAnonymous]
    [HttpGet("RefreshToken")]
    public ActionResult<List<object>> Refresh(string RefreshToken)
    {
        RefreshToken = Uri.UnescapeDataString(RefreshToken);
        RefreshToken = RefreshToken.Replace("\"", "");

        if (string.IsNullOrWhiteSpace(RefreshToken) || !(RefreshToken.Split('.').Length == 3))
        {
            return Unauthorized(new { message = "Invalid token format" });
        }

        JwtSecurityToken? claims = null;
        try
        {
            claims = new JwtSecurityTokenHandler().ReadJwtToken(RefreshToken);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = "Invalid token", exception = ex.Message });
        }

        if (claims == null)
        {
            return Unauthorized(new { message = "Invalid token format" });
        }

        var userData = new List<LdapUser>();

        userData.Add(new LdapUser
        {
            SAccout = claims.Claims.FirstOrDefault(c => c.Type == "SAccout")?.Value,
            UserName = claims.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value,
            Position = claims.Claims.FirstOrDefault(c => c.Type == "Position")?.Value,
            DepartMent = claims.Claims.FirstOrDefault(c => c.Type == "DepartMent")?.Value,
            UserID = claims.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value,
            GUserID = claims.Claims.FirstOrDefault(c => c.Type == "GUserID")?.Value,
            GUserName = claims.Claims.FirstOrDefault(c => c.Type == "GUserName")?.Value,
            Permissions = claims.Claims.Where(c => c.Type == "Permissions").Select(c => c.Value).ToArray() ?? new string[] { }
        });

        var token = _common.GenerateJwtToken(userData);

        return Ok(new { RefreshToken = token });
    }
}

