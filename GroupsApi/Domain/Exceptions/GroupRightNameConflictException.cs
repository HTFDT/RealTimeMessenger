using Core.Exceptions;

namespace Domain.Exceptions;

public class GroupRightNameConflictException : ConflictException
{
    public GroupRightNameConflictException(string name) 
        : base($"The group right with the name '{name}' already exists")
    {
    }
}