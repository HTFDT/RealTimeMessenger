using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class DefaultKickedRoleAsDefaultMemberRoleForbiddenException : ForbiddenException
{
    public DefaultKickedRoleAsDefaultMemberRoleForbiddenException(Guid id)
        : base($"Role with id '{id}' is default kicked member role, so it can't be used as default member role")
    {
    }
}