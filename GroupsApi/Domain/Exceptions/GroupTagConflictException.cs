using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class GroupTagConflictException : ConflictException
{
    public GroupTagConflictException(Guid groupId, Guid tagId)
        : base($"Group with the identifier {groupId} already has tag with the identifier {tagId}")
    {
    }
}