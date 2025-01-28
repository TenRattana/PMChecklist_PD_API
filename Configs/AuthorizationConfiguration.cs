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
                RoleClaimType = "Roles"
            };
        });

        services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmins", policy =>
        policy.RequireClaim("Permissions", "SuperAdmin"));

    options.AddPolicy("Admin", policy =>
        policy.RequireClaim("Permissions", "Admin"));

    options.AddPolicy("User", policy =>
        policy.RequireClaim("Permissions", "User"));

    options.AddPolicy("GroupUser", policy =>
        policy.RequireClaim("Permissions", "GroupUser"));

});

    }
}
