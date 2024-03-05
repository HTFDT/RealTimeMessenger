using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class OwnerLeaveForbiddenException : ForbiddenException
{
    public OwnerLeaveForbiddenException() 
        : base("Owner of the group can't leave")
    {
    }
}