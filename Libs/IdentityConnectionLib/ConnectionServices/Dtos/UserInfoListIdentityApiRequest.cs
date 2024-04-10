namespace IdentityConnectionLib.ConnectionServices.Dtos;

public record UserInfoListIdentityApiRequest(IEnumerable<Guid> UserIds);