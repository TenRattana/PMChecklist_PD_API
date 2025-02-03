using System.IdentityModel.Tokens.Jwt;
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
        var users = _ldapService.AuthenticateAsync(UserName, Password);

        if (users == null)
        {
            return NotFound("User not found.");
        }

        string token = _common.GenerateJwtToken(users);

        return Ok(new { token });
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshTokenRequest request)
    {
        JwtSecurityToken? claims = null;
        try
        {
            claims = new JwtSecurityTokenHandler().ReadJwtToken(request.AccessToken);
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
            Department = claims.Claims.FirstOrDefault(c => c.Type == "Department")?.Value,
            UserID = claims.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value,
            GUserID = claims.Claims.FirstOrDefault(c => c.Type == "GUserID")?.Value,
            GUserName = claims.Claims.FirstOrDefault(c => c.Type == "GUserName")?.Value,
            Permissions = claims.Claims.FirstOrDefault(c => c.Type == "Permissions")?.Value?.Split(',') ?? new string[] { }
        });

        var principal = _common.GenerateJwtToken(userData);

        var newRefreshToken = _common.GenerateRefreshToken();

        return Ok(new { RefreshToken = newRefreshToken });
    }
}

public class RefreshTokenRequest
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}


