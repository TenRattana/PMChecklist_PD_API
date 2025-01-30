using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using PMChecklist_PD_API.Models;

public class Common
{
    private readonly string _secretKey;

    public Common(IConfiguration configuration)
    {
        _secretKey = configuration["SECRET_KEY"] ?? "PMChecklst_PD_API";
    }

    public string GenerateJwtToken(List<LdapUser> users)
{
    if (users == null || !users.Any())
    {
        throw new ArgumentException("User list cannot be null or empty", nameof(users));
    }

    var user = users.First();

    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserID!),         
        new Claim(JwtRegisteredClaimNames.Name, user.UserName!),         
        new Claim("Department", user.Department!),                    
        new Claim("Position", user.Position!),                         
        new Claim("GUserID", user.GUserID!),                            
        new Claim("GUserName", user.GUserName!),                        
    };

    if (user.Permissions != null)
    {
        foreach (var permission in user.Permissions)
        {
            claims.Add(new Claim("Permissions", permission));
        }
    }

    if (string.IsNullOrEmpty(_secretKey))
    {
        throw new Exception("SECRET_KEY is not configured.");
    }

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.PadRight(32, '0')));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "PMChecklstIssuer",         
        audience: "PMChecklstAudience",     
        claims: claims,                     
        expires: DateTime.Now.AddHours(1),  
        signingCredentials: credentials     
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}


}
