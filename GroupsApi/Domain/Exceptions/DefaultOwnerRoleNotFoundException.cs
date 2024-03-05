using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class DefaultOwnerRoleNotFoundException : NotFoundException
{
    public DefaultOwnerRoleNotFoundException() : base("No default owner role set in system")
    {
    }
}