using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class AuthorizationConfiguration
{
    public static void AuthorizationConfigurationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration["SECRET_KEY"] ?? "PMChecklst_PD_API";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "PMChecklstIssuer",
                ValidAudience = "PMChecklstAudience",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey.PadRight(32, '0'))),
                RoleClaimType = ClaimTypes.Role
            };
        });

        services.AddAuthorization();

    }
}
