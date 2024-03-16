using System.Text;
using Api;
using Core.Base.Extensions;
using Core.Base.Jwt;
using Dal.Repository;
using Dal.Extensions;
using Dal.Repository.Interfaces;
using Logic;
using Logic.Models.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCustomSwagger();

builder.Services.AddDalServices(builder.Configuration);
builder.Configuration.SetBasePath("C:/Users/user/RiderProjects/RealTimeMessenger/")
    .AddJsonFile("Libs/Core/Base/Jwt/jwtoptions.json");
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
builder.Services.AddSingleton(jwtOptions);

builder.Services.AddCustomJwtAuthentication(jwtOptions);

builder.Services.AddCustomAuthorizationPolicies();

builder.Services.AddTransient<UserManager>();
builder.Services.AddTransient<JwtTokensManager>();
builder.Services.AddTransient<RoleManager>();


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