using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
            OnAuthenticationFailed = async context =>
            {
                var endpoint = context.HttpContext.GetEndpoint();
                var allowAnonymous = endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>();

                if (allowAnonymous != null)
                {
                    return;
                }

                if (context.Exception is SecurityTokenExpiredException)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var expiredTokenResponse = new { message = "Token expired. Please refresh the token." };
                    await context.Response.WriteAsJsonAsync(expiredTokenResponse);
                    return;
                }

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var response = new { message = "Invalid token or authentication failed." };
                await context.Response.WriteAsJsonAsync(response);
            },

            OnChallenge = context =>
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var response = new { message = "Unauthorized. Please provide a valid token." };
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

        services.AddHttpContextAccessor();
    }
}
