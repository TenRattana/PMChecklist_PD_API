using DotNetEnv;

public static class ConfigurationExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        Env.Load();

        services.AddDatabaseServices(configuration);

        services.AddControllers();
        services.AddSingleton<Connection>();

        services.AddScoped<LogService>();

        services.AddScoped<Common>();
        services.AddScoped<GroupMachineService>();
        services.AddScoped<MachineService>();
        services.AddScoped<CheckListService>();
        services.AddScoped<CheckListOptionService>();
        services.AddScoped<GroupCheckListService>();
        services.AddScoped<MatchCheckListService>();
        services.AddScoped<MatchCheckListOptionService>();
        services.AddScoped<MatchFormMachineService>();
        services.AddScoped<ExpectedResultService>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerServices();
        services.AddLdapServices();

        services.AuthorizationConfigurationServices(configuration);
    }

}
