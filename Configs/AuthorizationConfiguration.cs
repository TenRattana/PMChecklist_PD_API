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

            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";

                    var response = new { message = "Unauthorized. Please provide a valid token." };
                    return context.Response.WriteAsJsonAsync(response);
                },
                OnAuthenticationFailed = context =>
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var response = new { message = "Invalid token or token expired." };
                    return context.Response.WriteAsJsonAsync(response);
                }
            };

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "PMChecklstIssuer",
                ValidAudience = "PMChecklstAudience",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey.PadRight(32, '0'))),
                RoleClaimType = "Permissions"
            };
        });

        services.AddAuthorization();

    }
}
