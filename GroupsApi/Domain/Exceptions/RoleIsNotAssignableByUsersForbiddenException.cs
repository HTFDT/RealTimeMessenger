using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class RoleIsNotAssignableByUsersForbiddenException : ForbiddenException
{
    public RoleIsNotAssignableByUsersForbiddenException(Guid roleId) 
        : base($"The role with the identifier '{roleId}' is not assignable by users")
    {
    }
}