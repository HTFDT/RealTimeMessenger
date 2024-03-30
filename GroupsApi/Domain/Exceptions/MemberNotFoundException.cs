using Core.Exceptions;

namespace Domain.Exceptions;

public class MemberNotFoundException : NotFoundException
{
    public MemberNotFoundException(Guid memberId, Guid groupId) 
        : base($"The member with the identifier '{memberId}' was not found in the group with the identifier '{groupId}'")
    {
    }
}