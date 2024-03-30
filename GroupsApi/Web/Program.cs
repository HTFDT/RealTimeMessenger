using Core.HttpLogic.Extensions;
using Core.Jwt;
using Core.Jwt.Extensions;
using Core.Middleware;
using Core.Swagger.Extensions;
using Core.TraceIdLogic.Extensions;
using IdentityConnectionLib.ConnectionServices.Extensions;
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
    .AddJsonFile("Libs/Core/Jwt/jwtoptions.json");

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
builder.Services.AddCustomJwtAuthentication(jwtOptions);
builder.Services.AddCustomAuthorizationPolicies();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddTransient<TraceWriterMiddleware>();
builder.Services.AddTransient<TraceIdMiddleware>();

builder.Services.AddHttpRequestService();
builder.Services.AddIdentityConnectionService();
builder.Services.TryAddTraceId();
builder.Services.TryAddTraceJwtToken();

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
app.UseMiddleware<TraceWriterMiddleware>();
app.UseMiddleware<TraceIdMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();