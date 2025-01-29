using NLog;
using NLog.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();
builder.Logging.AddNLog();

builder.Services.ConfigureServices(builder.Configuration);

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

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

// app.Use(async (context, next) =>
// {
//     var routeData = context.GetRouteData();
//     var controllerName = routeData?.Values["controller"]?.ToString();

//     if (!string.IsNullOrEmpty(controllerName) && controllerName.StartsWith("Bearer "))
//     {
//         var token = controllerName.Substring("Bearer ".Length).Trim();

//         var handler = new JwtSecurityTokenHandler();
//         var jwtToken = handler.ReadJwtToken(token);

//         var username = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
//         var role = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

//         var logger = LogManager.GetCurrentClassLogger();
//         logger.Info($"Token decoded: Username = {username}, Role = {role}");
//     }

//     await next.Invoke();
// });

app.MapControllers();

app.Run();
