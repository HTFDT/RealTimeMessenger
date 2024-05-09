using MassTransit;
using web.Sagas.Memberships;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddSagaStateMachine<MembershipStateMachine, MembershipState>()
        .InMemoryRepository();
    
    cfg.UsingRabbitMq((context, rmq) =>
    {
        rmq.Host("localhost");
        rmq.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.Run();