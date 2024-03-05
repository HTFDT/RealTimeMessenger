using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class RoleIsNotRevocableForbiddenException : ForbiddenException
{
    public RoleIsNotRevocableForbiddenException(Guid groupRoleId)
        : base($"The role with the identifier '{groupRoleId}' is not revocable")
    {
    }
}