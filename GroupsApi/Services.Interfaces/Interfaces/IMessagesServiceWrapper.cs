using Shared.Dto.Messages;

namespace Services.Interfaces.Interfaces;

public interface IMessagesServiceWrapper
{
    Task<IEnumerable<MessageResponse>> GetMessagesInGroup(Guid requesterId, Guid groupId, GetMessagesRequest request);
    Task<MessageResponse> GetMessageInGroup(Guid requesterId, Guid groupId, long groupNumber);
    Task<MessageResponse> GetMessageInGroup(Guid requesterId, Guid groupId, Guid messageId);
    Task<MessageResponse> CreateMessage(Guid requesterId, Guid groupId, CreateMessageRequest request);
    Task<MessageResponse> EditMessage(Guid requesterId, Guid groupId, long groupNumber, EditMessageRequest request);
    Task<MessageResponse> EditMessage(Guid requesterId, Guid groupId, Guid messageId, EditMessageRequest request);
    Task DeleteMessage(Guid requesterId, Guid groupId, long groupNumber);
    Task DeleteMessage(Guid requesterId, Guid groupId, Guid messageId);
    Task<MessageResponse> PinMessage(Guid requesterId, Guid groupId, Guid messageId);
    Task<MessageResponse> PinMessage(Guid requesterId, Guid groupId, long groupNumber);
    Task<MessageResponse> UnpinMessage(Guid requesterId, Guid groupId, Guid messageId);
    Task<MessageResponse> UnpinMessage(Guid requesterId, Guid groupId, long groupNumber);
}