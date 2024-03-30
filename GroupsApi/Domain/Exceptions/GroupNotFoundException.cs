using Core.Exceptions;

namespace Domain.Exceptions;

public class GroupNotFoundException : NotFoundException
{
    public GroupNotFoundException(Guid id)
        : base($"The group with the identifier {id} was not found.")
    {
    }
}