using System.Net;
using Core.HttpLogic;
using Core.HttpLogic.Interfaces;
using IdentityConnectionLib.ConnectionServices.Dtos;
using IdentityConnectionLib.ConnectionServices.Interfaces;
using IdentityConnectionLib.Exceptions;

namespace IdentityConnectionLib.ConnectionServices;

internal class HttpIdentityConnectionService(IHttpRequestService httpRequestService) : IIdentityConnectionService
{
    private const string BasePath = "https://localhost:7071";
    public async Task<UserInfoIdentityApiResponse> GetUserInfo(UserInfoIdentityApiRequest request)
    {
        var requestData = new HttpRequestData
        {
            Body = null,
            Method = HttpMethod.Get,
            Path = BasePath + $"/users/{request.UserId}"
        };
        var response = await httpRequestService.SendRequestAsync<UserInfoIdentityApiResponse>(requestData);
        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new UserInfoNotFoundException(request.UserId);
        return response.Body!;
    }
    
    public async Task<IList<UserInfoListIdentityApiResponse>> GetUserInfos(UserInfoListIdentityApiRequest request)
    {
        var requestData = new HttpRequestData
        {
            Body = null,
            Method = HttpMethod.Get,
            Path = BasePath + "/users"
        };
        var response = await httpRequestService.SendRequestAsync<IEnumerable<UserInfoListIdentityApiResponse>>(requestData);
        var idsToReturn = request.UserIds.ToHashSet();
        var res = new List<UserInfoListIdentityApiResponse>();
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