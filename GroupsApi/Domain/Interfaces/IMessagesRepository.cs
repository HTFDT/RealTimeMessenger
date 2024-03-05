using Domain.Entities;

namespace Domain.Interfaces;

public interface IMessagesRepository : IRepository<Message>
{
    Task<Message?> GetByGroupIdAndGroupNumber(Guid groupId, long groupNumber);
    Task<Message?> GetByGroupIdAndMessageId(Guid groupId, Guid messageId);
    Task<List<Message>> GetAllMessagesInGroupAsync(Guid groupId);
    Task<List<Message>> GetAllMessagesInGroupFromUserAsync(Guid groupId, Guid userId);
    Task<List<Message>> GetAllMessagesFromUserAsync(Guid userId);
    Task<List<Message>> GetMessagesByGroupNum(Guid groupId, long? fromNum, long? toNum);
    Task<List<Message>> GetMessagesByDate(Guid groupId, DateTime? fromDate, DateTime? toDate);
}