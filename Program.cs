using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PMChecklist_PD_API.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()  
    .WriteTo.Console()  
    .WriteTo.File("logs/myapp.log", rollingInterval: RollingInterval.Day) 
    .CreateLogger();

builder.Logging.ClearProviders(); 
builder.Logging.AddSerilog(); 

builder.Logging.AddFilter("Microsoft", LogLevel.Warning) 
               .AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None) 
               .AddFilter("Microsoft.AspNetCore", LogLevel.Warning)
               .AddFilter("System", LogLevel.Warning); 

builder.Services.AddRazorPages();

builder.Services.AddDbContext<PCMhecklistContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors(true)
);

builder.Services.AddSingleton<Connection>();

builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
