using System.Diagnostics;
using Core.MassTransit.Messages;
using MassTransit;

namespace web.Sagas.Memberships;

public class MembershipStateMachine : MassTransitStateMachine<MembershipState>
{
    public Event<ProcessMembership> MembershipRequested { get; set; } = null!;
    public Request<MembershipState, MembershipRequest, MembershipResponse> ProcessMembership { get; set; } = null!;
    public Request<MembershipState, UserInfoRequest, UserInfoResponse> ProcessUserInfo { get; set; } = null!;
    
    public MembershipStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        Request(() => ProcessMembership, x => x.ProcessingId, cfg => cfg.Timeout = TimeSpan.Zero);
        Request(() => ProcessUserInfo, x => x.ProcessingId, cfg => cfg.Timeout = TimeSpan.Zero);
        Event(() => MembershipRequested, 
            x => x.CorrelateById(context => context.Message.MembershipId));
        
        // по получении запроса сохраняем данные об обратном адресе и ставим его в пендинг
        Initially(
            When(MembershipRequested)
                .Then(context =>
                {
                    context.Saga.ResponseAddress = context.ResponseAddress;
                    context.Saga.RequestId = context.RequestId;
                })
                // посылаем запрос на сервис групп для получения сущности члена группы
                .Request(ProcessMembership, context => new MembershipRequest
                {
                    RequesterId = context.Message.RequesterId,
                    GroupId = context.Message.GroupId,
                    MembershipId = context.Message.MembershipId
                })
                .TransitionTo(ProcessMembership.Pending));
        
        
        During(ProcessMembership.Pending,
            // получаем сущность члена группы и сохраняем в инстансе машины состояний
            When(ProcessMembership.Completed)
                .Then(context =>
                {
                    context.Saga.MembershipResponse = context.Message;
                })
                // посылаем запрос на сервис пользователей, для получения имени пользователя
                .Request(ProcessUserInfo, context => new UserInfoRequest { Id = context.Message.UserId })
                .TransitionTo(ProcessUserInfo.Pending),
            When(ProcessMembership.Faulted)
                .Then(context =>
                {
                    foreach (var e in context.Message.Exceptions)
                        Console.WriteLine(e);
                })
                .Finalize());
        
        During(ProcessUserInfo.Pending,
            When(ProcessUserInfo.Completed)
                // после получения имени пользователя объединяем ответы и отвечаем на изначальный зарпос
                .ThenAsync(async context =>
                {
                    var endpoint = await context.GetSendEndpoint(context.Saga.ResponseAddress!);
                    await endpoint.Send(new MembershipWithUsernameResponse
                    {
                        Membership = context.Saga.MembershipResponse,
                        Username = context.Message.Username
                    }, r => r.RequestId = context.Saga.RequestId);
                })
                .Finalize(),
            When(ProcessUserInfo.Faulted)
                .Then(context =>
                {
                    foreach (var e in context.Message.Exceptions)
                        Console.WriteLine(e);
                })
                .Finalize());
        
        SetCompletedWhenFinalized();
    }
}