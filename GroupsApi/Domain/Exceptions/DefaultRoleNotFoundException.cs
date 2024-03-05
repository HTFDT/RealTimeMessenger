using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class DefaultRoleNotFoundException : NotFoundException
{
    public DefaultRoleNotFoundException(Guid id) 
        : base($"The default role with the identifier {id} was not found")
    {
    }
}