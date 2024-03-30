using Core.Exceptions;

namespace Domain.Exceptions;

public class JoinPrivateGroupIsForbiddenException : ForbiddenException
{
    public JoinPrivateGroupIsForbiddenException(Guid groupId)
        : base($"Can't join the private group with the identifier '{groupId}")
    {
    }
}