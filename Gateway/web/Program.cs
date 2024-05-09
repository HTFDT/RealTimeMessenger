using Core.Jwt;
using Core.Jwt.Extensions;
using Core.Swagger.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, rmq) =>
    {
        rmq.Host("localhost");
        rmq.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();

builder.Services.AddCustomSwagger()
    .AddEndpointsApiExplorer();
    
builder.Configuration
    .SetBasePath("C:/Users/user/RiderProjects/RealTimeMessenger/")
    .AddJsonFile("Libs/Core/Jwt/jwtoptions.json");

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
builder.Services.AddCustomJwtAuthentication(jwtOptions);
builder.Services.AddCustomAuthorizationPolicies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();