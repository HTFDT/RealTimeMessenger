using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class NotAMemberNotFoundException : NotFoundException
{
    public NotAMemberNotFoundException(Guid userId, Guid groupId) 
        : base($"The user with the identifier '{userId}' is not a member of the group with the identifier '{groupId}'")
    {
    }
}