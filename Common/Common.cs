using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

public class Common
{
    private readonly string _secretKey;

    public Common(IConfiguration configuration)
    {
        _secretKey = configuration["SECRET_KEY"] ?? "PMChecklst_PD_API";
    }

    public string GenerateJwtToken(string username, string role)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("Username cannot be null or empty", nameof(username));
        }

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, role)
        };


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
