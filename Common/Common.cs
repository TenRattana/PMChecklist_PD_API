using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

public class Common
{
    private readonly IConfiguration _configuration;

    public Common(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(string username)
    {
        // ตรวจสอบว่า username ไม่เป็นค่าว่าง
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("Username cannot be null or empty", nameof(username));
        }

        var claims = new[] {
            new Claim(ClaimTypes.Name, username),
        };

        var secretKey = _configuration["SECRET_KEY"] ?? Environment.GetEnvironmentVariable("SECRET_KEY");

        // ตรวจสอบว่า SECRET_KEY ถูกตั้งค่าหรือไม่
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new Exception("SECRET_KEY is not configured.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "your_issuer",        
            audience: "your_audience",     
            claims: claims,               
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
