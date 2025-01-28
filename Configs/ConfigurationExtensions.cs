using DotNetEnv;

public static class ConfigurationExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        Env.Load();

        services.AddDatabaseServices(configuration);

        services.AddControllers();
        services.AddScoped<Connection>();
        services.AddScoped<Common>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddEndpointsApiExplorer();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        services.AddSwaggerServices();

        services.AddLdapServices();
        services.AuthorizationConfigurationServices(configuration);
    }

}
