using System.Net;
using Core.HttpLogic;
using Core.HttpLogic.Interfaces;
using IdentityConnectionLib.ConnectionServices.Dtos;
using IdentityConnectionLib.ConnectionServices.Interfaces;
using IdentityConnectionLib.Exceptions;

namespace IdentityConnectionLib.ConnectionServices;

internal class IdentityConnectionService(IHttpRequestService httpRequestService) : IIdentityConnectionService
{
    private const string BasePath = "https://localhost:7071";
    public async Task<UserInfoResponse> GetUserInfo(Guid userId)
    {
        var requestData = new HttpRequestData
        {
            Body = null,
            Method = HttpMethod.Get,
            Path = BasePath + $"/users/{userId}"
        };
        var response = await httpRequestService.SendRequestAsync<UserInfoResponse>(requestData);
        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new UserInfoNotFoundException(userId);
        return response.Body!;
    }
    
    public async Task<IList<UserInfoResponse>> GetUserInfos(IEnumerable<Guid> userIds)
    {
        var requestData = new HttpRequestData
        {
            Body = null,
            Method = HttpMethod.Get,
            Path = BasePath + "/users"
        };
        var response = await httpRequestService.SendRequestAsync<IEnumerable<UserInfoResponse>>(requestData);
        var idsToReturn = userIds.ToHashSet();
        var res = new List<UserInfoResponse>();
        foreach (var user in response.Body!)
        {
            if (!idsToReturn.Contains(user.Id))
                throw new UserInfoNotFoundException(user.Id);
            res.Add(user);
            idsToReturn.Remove(user.Id);
        }
        
        return res;
    }
}