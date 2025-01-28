using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

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

// app.UseHttpsRedirection();
// app.UseCors("AllowAllOrigins");app.UseCors(builder =>
// {
//     builder.WithOrigins("http://localhost:5170") 
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .AllowCredentials();
// });

app.UseAuthentication();
app.UseAuthorization(); 

app.Use(async (context, next) =>
{
    var routeData = context.GetRouteData();
    var controllerName = routeData?.Values["controller"]?.ToString();

    // if (controllerName == "GroupUsers")
    // {
    //     var authHeader = context.Request.Headers["Authorization"].ToString();

    //     if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
    //     {
    //         var token = authHeader.Substring("Bearer ".Length).Trim();
    //         var handler = new JwtSecurityTokenHandler();
    //         var jwtToken = handler.ReadJwtToken(token);

    //         var username = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
    //         var rolesClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "Roles")?.Value;

    //         if (!string.IsNullOrEmpty(rolesClaim))
    //         {
    //             var roles = rolesClaim.Split(',');
    //         }
    //     }
    // }

    await next.Invoke();
});


app.MapControllers();

app.Run();
