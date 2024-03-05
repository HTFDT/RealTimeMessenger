using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class MemberIsAlreadyInGroupConflictException : ConflictException
{
    public MemberIsAlreadyInGroupConflictException(Guid userId)
        : base($"The user with the identifier '{userId}' is already a member of this group")
    {
    }
}