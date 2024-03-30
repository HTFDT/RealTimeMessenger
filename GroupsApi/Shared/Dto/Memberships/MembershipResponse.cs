namespace Shared.Dto.Memberships;

public record MembershipResponse(Guid Id,
    Guid UserId,
    string Username,
    Guid GroupId,
    Guid? GroupRoleId, 
    DateTime DateJoined,
    bool IsKicked,
    DateTime? DateKicked);