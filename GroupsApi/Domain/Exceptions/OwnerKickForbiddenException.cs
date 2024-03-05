using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class OwnerKickForbiddenException : ForbiddenException
{
    public OwnerKickForbiddenException() 
        : base("The owner of the group can't be kicked")
    {
    }
}