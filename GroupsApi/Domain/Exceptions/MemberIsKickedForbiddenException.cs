using Core.Exceptions;

namespace Domain.Exceptions;

public class MemberIsKickedForbiddenException : ForbiddenException
{
    public MemberIsKickedForbiddenException(Guid userId)
        : base($"The user with the identifier '{userId}' was kicked from this group. " +
               $"He needs to be unbanned to be in this group again.")
    {
    }
}