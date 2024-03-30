using Core.Exceptions;

namespace Domain.Exceptions;

public class TagNameConflictException : ConflictException
{
    public TagNameConflictException(string tagName) 
        : base($"The tag with the name '{tagName}' already exists")
    {
    }
}