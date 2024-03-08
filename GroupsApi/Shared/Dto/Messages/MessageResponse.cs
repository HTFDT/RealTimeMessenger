namespace Shared.Dto.Messages;

public record MessageResponse(Guid Id, 
    Guid SenderId, 
    Guid GroupId, 
    long? GroupNumber,
    DateTime DateSent,
    DateTime? DateEdited,
    Guid? ReplyTo,
    bool IsPinned,
    string Text);