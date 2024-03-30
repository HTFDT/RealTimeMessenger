using Core.Exceptions;

namespace Domain.Exceptions;

public class DefaultUniqueRoleForbiddenException : ForbiddenException
{
    public DefaultUniqueRoleForbiddenException(Guid id) 
        : base($"Role with id '{id}' is unique, so it can't be used as default member role")
    {
    }
}