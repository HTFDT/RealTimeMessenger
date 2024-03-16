using Core.Base.Extensions;
using Core.Base.Jwt;
using Core.Base.Middleware;
using Infrastructure.Extensions;
using Presentation;
using Services.Extensions;
using Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(AssemblyRef).Assembly);

builder.Services.AddCustomSwagger();
    
builder.Configuration
    .SetBasePath("C:/Users/user/RiderProjects/RealTimeMessenger/")
    .AddJsonFile("Libs/Core/Base/Jwt/jwtoptions.json");

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
builder.Services.AddCustomJwtAuthentication(jwtOptions);
builder.Services.AddCustomAuthorizationPolicies();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();