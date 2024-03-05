using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class DefaultLeftRoleNotFoundException : NotFoundException
{
    public DefaultLeftRoleNotFoundException() 
        : base("No default left member role set in system")
    {
    }
}