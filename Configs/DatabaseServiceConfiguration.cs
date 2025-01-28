using Microsoft.EntityFrameworkCore;
using PMChecklist_PD_API.Models;

public static class DatabaseServiceConfiguration
{
    public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<PCMhecklistContext>(options => options.UseSqlServer(connectionString));
    }
}
