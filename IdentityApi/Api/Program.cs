using Api;
using Api.Consumers;
using Core.Jwt;
using Core.Jwt.Extensions;
using Core.Swagger.Extensions;
using Dal.Extensions;
using Logic;
using Api.Consumers.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCustomSwagger();

builder.Services.AddDalServices(builder.Configuration);
builder.Configuration.SetBasePath("C:/Users/user/RiderProjects/RealTimeMessenger/")
    .AddJsonFile("Libs/Core/Jwt/jwtoptions.json");
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
builder.Services.AddSingleton(jwtOptions);

builder.Services.AddCustomJwtAuthentication(jwtOptions);

builder.Services.AddCustomAuthorizationPolicies();

builder.Services.AddTransient<UserManager>();
builder.Services.AddTransient<JwtTokensManager>();
builder.Services.AddTransient<RoleManager>();

builder.Services.AddConsumers();
builder.Services.AddHostedService<IdentityConsumptionManagerService>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumers(typeof(Program).Assembly);
    
    cfg.UsingRabbitMq((context, rmq) =>
    {   
        rmq.Host("localhost");
        rmq.ConfigureEndpoints(context);
    });
});



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services, builder.Configuration.GetValue<string>("SeedUserPw")!);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();