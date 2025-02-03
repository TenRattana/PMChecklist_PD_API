using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using PMChecklist_PD_API.Models;
using System.Security.Cryptography;

public class Common
{
    private readonly string _secretKey;
    private readonly Connection _connection;

    public Common(IConfiguration configuration, Connection connection)
    {
        _secretKey = configuration["SECRET_KEY"] ?? "PMChecklst_PD_API";
        _connection = connection;
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
        new Claim("UserID", user.UserID!),
        new Claim("SAccout", user.UserID!),
        new Claim("UserName", user.UserName!),
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

    public string GenerateRefreshToken()
    {
        var tokenData = $"{Guid.NewGuid()}_{DateTime.UtcNow.Ticks}";

        var tokenBytes = Encoding.UTF8.GetBytes(tokenData);

        return Convert.ToBase64String(tokenBytes);
    }

    public int GetMaxID(string tableName, string columnName, string prefix)
    {
        var maxValue = 0;

        var query = $"SELECT MAX(CAST(SUBSTRING({columnName}, LEN('{prefix}') + 1, LEN({columnName}) - LEN('{prefix}')) AS INT)) AS MaxValue FROM {tableName} WHERE {columnName} LIKE '{prefix}' + '%'";

        var max = _connection.QueryData<dynamic>(query, new { });

        if (max.Any() && max.First().MaxValue != null)
        {
            maxValue = max.First().MaxValue;
        }

        return maxValue;
    }

    public string FormattedId(string prefix, int maxId, int length = 9)
    {
        int numberLength = length - prefix.Length;

        if (numberLength < 1)
        {
            numberLength = 0;
        }

        string result = prefix + maxId.ToString("D" + numberLength);

        if (result.Length > length)
        {
            result = result.Substring(0, length);
        }

        return result;
    }

}
