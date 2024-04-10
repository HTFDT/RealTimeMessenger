using IdentityConnectionLib.ConnectionServices.Dtos;

namespace IdentityConnectionLib.ConnectionServices.Interfaces;

public interface IIdentityConnectionService
{
    Task<UserInfoIdentityApiResponse> GetUserInfo(UserInfoIdentityApiRequest request);
    Task<IList<UserInfoListIdentityApiResponse>> GetUserInfos(UserInfoListIdentityApiRequest request);
}