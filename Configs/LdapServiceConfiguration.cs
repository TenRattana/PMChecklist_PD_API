using PMChecklist_PD_API.Services;

public static class LdapServiceConfiguration
{
    public static void AddLdapServices(this IServiceCollection services)
    {
        services.AddScoped<LdapService>();
    }
}
