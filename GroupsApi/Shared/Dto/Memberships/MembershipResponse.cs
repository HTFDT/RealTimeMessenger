namespace Shared.Dto.Memberships;

public record MembershipResponse(Guid Id,
    Guid UserId,
    Guid GroupId,
    Guid? GroupRoleId, 
    DateTime DateJoined,
    bool IsKicked,
    DateTime? DateKicked);