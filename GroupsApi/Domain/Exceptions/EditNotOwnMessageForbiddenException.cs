using Core.Exceptions;

namespace Domain.Exceptions;

public class EditNotOwnMessageForbiddenException : ForbiddenException
{
    public EditNotOwnMessageForbiddenException(Guid messageId) 
        : base($"The message with the identifier '{messageId}' isn't yours, so you can't edit it")
    {
    }
    
    public EditNotOwnMessageForbiddenException(long groupNum) 
        : base($"The message with the group number '{groupNum}' isn't yours, so you can't edit it")
    {
    }
}