using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class DefaultKickedRoleNotFoundException : NotFoundException
{
    public DefaultKickedRoleNotFoundException() 
        : base("No default kicked role set in system")
    {
    }
}