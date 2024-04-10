using System.Text.Json;
using Core.RabbitLogic.Interfaces;
using Core.ServiceIdLogic.Interfaces;
using IdentityConnectionLib.ConnectionServices.Dtos;
using IdentityConnectionLib.ConnectionServices.Interfaces;
using IdentityConnectionLib.Exceptions;
using RabbitMQ.Client;

namespace IdentityConnectionLib.ConnectionServices;

/// <summary>
/// синлглтон, который хранит в себе подключение Rabbit на протяжении всего времени жизни микросервиса.
/// Взаимозаменим с аналогичным http-сервисом
/// </summary>
internal class RabbitMqIdentityConnectionService : IIdentityConnectionService, IDisposable
{
    private readonly Lazy<IConnection> _connection;
    private readonly IRabbitMqClientService _clientService;
    private readonly IServiceIdAccessor _serviceIdAccessor;

    public RabbitMqIdentityConnectionService(IRabbitMqClientService clientService, IServiceIdAccessor serviceIdAccessor)
    {
        _serviceIdAccessor = serviceIdAccessor;
        _clientService = clientService;
        _connection = new Lazy<IConnection>(() =>
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _clientService.Consume(connection,
                "IdentityApiResponse", 
                _serviceIdAccessor.ServiceId.ToString(),
                ExchangeType.Direct);
            return connection;
        });
    }

    public async Task<UserInfoIdentityApiResponse> GetUserInfo(UserInfoIdentityApiRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var result = await _clientService.ProduceAsync(_connection.Value,
            json,
            nameof(UserInfoIdentityApiRequest),
            nameof(UserInfoIdentityApiRequest),
            ExchangeType.Direct,
            _serviceIdAccessor.ServiceId.ToString());
        var response = JsonSerializer.Deserialize<UserInfoIdentityApiResponse>(result);
        if (response is null)
            throw new UserInfoNotFoundException(request.UserId);
        return response;
    }

    public async Task<IList<UserInfoListIdentityApiResponse>> GetUserInfos(UserInfoListIdentityApiRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var result = await _clientService.ProduceAsync(_connection.Value,
            json,
            nameof(UserInfoListIdentityApiRequest),
            nameof(UserInfoListIdentityApiRequest),
            ExchangeType.Direct,
            _serviceIdAccessor.ServiceId.ToString());
        var response = JsonSerializer.Deserialize<IEnumerable<UserInfoListIdentityApiResponse>>(result);
        var idsToReturn = request.UserIds.ToHashSet();
        var res = new List<UserInfoListIdentityApiResponse>();
        foreach (var user in response!)
        {
            if (!idsToReturn.Contains(user.Id))
                throw new UserInfoNotFoundException(user.Id);
            res.Add(user);
            idsToReturn.Remove(user.Id);
        }
        
        return res;
    }

    public void Dispose()
    {
        if (_connection.IsValueCreated)
            _connection.Value.Close();
    }
}