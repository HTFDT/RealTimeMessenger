using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class MessageNotFoundException : NotFoundException
{
    public MessageNotFoundException(Guid groupId, Guid messageId)
        : base($"The message with the identifier '{messageId}' was not found in the group with the identifier '{groupId}'")
    {
    }
    
    public MessageNotFoundException(Guid groupId, long groupNumber)
        : base($"The message with the group number '{groupNumber}' was not found in the group with the identifier '{groupId}'")
    {
    }
}