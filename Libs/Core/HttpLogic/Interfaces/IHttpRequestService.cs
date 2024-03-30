using Microsoft.IdentityModel.Protocols;

namespace Core.HttpLogic.Interfaces;

public interface IHttpRequestService
{
    /// <summary>
    /// Отправить HTTP-запрос
    /// </summary>
    Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(HttpRequestData requestData,
        HttpConnectionData connectionData = default);
}