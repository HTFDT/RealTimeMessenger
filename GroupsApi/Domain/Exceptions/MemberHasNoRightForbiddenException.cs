using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class MemberHasNoRightForbiddenException : ForbiddenException
{
    public MemberHasNoRightForbiddenException(Guid membershipId, string right)
        : base($"The member with the identifier {membershipId} has no right ({right}) to perform this action")
    {
    }
}