using Core.Exceptions;

namespace Domain.Exceptions;

public class GroupRoleNotFoundException : NotFoundException
{
    public GroupRoleNotFoundException(Guid groupId, Guid roleId) 
        : base($"The group role with the identifier {roleId} in the group with the identifier {groupId} " + 
               $"was not found.")
    {
    }
}