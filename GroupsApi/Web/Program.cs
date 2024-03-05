using Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(AssemblyRef).Assembly);

var app = builder.Build();


app.Run();