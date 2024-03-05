using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class GroupRightNotFoundException : NotFoundException
{
    public GroupRightNotFoundException(Guid id) 
        : base($"The group right with the identifier {id} was not found.")
    {
    }
}