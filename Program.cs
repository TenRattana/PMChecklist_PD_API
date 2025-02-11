using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PMChecklist_PD_API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Services.AddLogging(config =>
{
    config.AddConsole();  // ใช้การบันทึกออกทาง Console
    config.AddDebug();    // ใช้การบันทึกใน Debug output window
    config.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information); // เฉพาะคำสั่งที่ถูกส่งไปยังฐานข้อมูล
});

builder.Services.AddDbContext<PCMhecklistContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging() // เปิดการบันทึกข้อมูลที่ละเอียด เช่นค่า parameter ใน SQL
           .LogTo(Console.WriteLine, LogLevel.Information) // log ทุกคำสั่ง SQL ไปที่ Console
);

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

// app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
