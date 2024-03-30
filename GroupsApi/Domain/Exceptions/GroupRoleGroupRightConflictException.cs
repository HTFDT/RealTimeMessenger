using Core.Exceptions;

namespace Domain.Exceptions;

public class GroupRoleGroupRightConflictException : ConflictException
{
    public GroupRoleGroupRightConflictException(Guid groupRoleId, Guid groupRightId) 
        : base($"The group role with the identifier {groupRoleId} already has the right with the identifier {groupRightId}")
    {
    }
}