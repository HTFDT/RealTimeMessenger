using Shared.Dto.Messages;

namespace Services.Interfaces.Interfaces;

public interface IMessagesService
{
    Task<IEnumerable<MessageResponse>> GetMessagesInGroupAfterJoin(Guid requesterId, Guid groupId, long? groupNumFrom, long? groupNumTo);
    Task<IEnumerable<MessageResponse>> GetMessagesInGroupAfterJoin(Guid requesterId, Guid groupId, DateTime? dateFrom, DateTime? dateTo);
    Task<IEnumerable<MessageResponse>> GetMessagesInGroupBeforeJoin(Guid requesterId, Guid groupId, long? groupNumFrom, long? groupNumTo);
    Task<IEnumerable<MessageResponse>> GetMessagesInGroupBeforeJoin(Guid requesterId, Guid groupId, DateTime? dateFrom, DateTime? dateTo);
    Task<MessageResponse> GetMessageInGroupBeforeJoin(Guid requesterId, Guid groupId, Guid messageId);
    Task<MessageResponse> GetMessageInGroupBeforeJoin(Guid requesterId, Guid groupId, long groupNum);
    Task<MessageResponse> GetMessageInGroupAfterJoin(Guid requesterId, Guid groupId, Guid messageId);
    Task<MessageResponse> GetMessageInGroupAfterJoin(Guid requesterId, Guid groupId, long groupNum);
    Task<MessageResponse> CreateMessage(Guid requesterId, Guid groupId, CreateMessageRequest request);
    Task<MessageResponse> EditMessage(Guid requesterId, Guid groupId, long groupNumber, EditMessageRequest request);
    Task<MessageResponse> EditMessage(Guid requesterId, Guid groupId, Guid messageId, EditMessageRequest request);
    Task DeleteOwnMessage(Guid requesterId, Guid groupId, long groupNumber);
    Task DeleteOwnMessage(Guid requesterId, Guid groupId, Guid messageId);
    Task DeleteSomeoneElseMessage(Guid requesterId, Guid groupId, long groupNumber);
    Task DeleteSomeoneElseMessage(Guid requesterId, Guid groupId, Guid messageId);
    Task<MessageResponse> PinMessage(Guid requesterId, Guid groupId, Guid messageId);
    Task<MessageResponse> PinMessage(Guid requesterId, Guid groupId, long groupNumber);
    Task<MessageResponse> UnpinMessage(Guid requesterId, Guid groupId, Guid messageId);
    Task<MessageResponse> UnpinMessage(Guid requesterId, Guid groupId, long groupNumber);
}