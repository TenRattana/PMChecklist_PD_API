using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PMChecklist_PD_API.Models;
using System.Reflection;
using DotNetEnv;
using PMChecklist_PD_API.Services;

public static class ConfigurationExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        Env.Load();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<PCMhecklistContext>(options => options.UseSqlServer(connectionString));

        services.AddControllers();
        services.AddScoped<Connection>();
        services.AddScoped<Common>();
        services.AddScoped<LdapService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddEndpointsApiExplorer();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "PMChecklist Doc API",
                Description = "ASP.NET Core Web API for managing PMChecklist",
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

    }

    public static void ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseSwagger(options =>
        {
            options.SerializeAsV2 = true;
        });

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });

        app.UseCors("AllowAllOrigins");

        app.UseHttpsRedirection();

        app.UseAuthorization();
    }
}
