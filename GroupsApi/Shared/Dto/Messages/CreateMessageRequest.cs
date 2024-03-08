namespace Shared.Dto.Messages;

public record CreateMessageRequest(string Text, Guid? ReplyTo);