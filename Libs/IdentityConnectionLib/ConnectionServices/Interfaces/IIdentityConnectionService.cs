using IdentityConnectionLib.ConnectionServices.Dtos;

namespace IdentityConnectionLib.ConnectionServices.Interfaces;

public interface IIdentityConnectionService
{
    Task<UserInfoResponse> GetUserInfo(Guid userId);
    Task<IList<UserInfoResponse>> GetUserInfos(IEnumerable<Guid> userIds);
}