using Core.Exceptions;

namespace Domain.Exceptions;

public class DefaultMemberRoleNotFoundException : NotFoundException
{
    public DefaultMemberRoleNotFoundException() : base("No default member role set in system")
    {
    }
}