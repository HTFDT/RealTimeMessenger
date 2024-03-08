using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class MessagesRepository(ApplicationDbContext context) : BaseRepository<Message>(context), IMessagesRepository
{
    public Task<Message?> GetByGroupIdAndGroupNumber(Guid groupId, long groupNumber)
    {
        return Entities.SingleOrDefaultAsync(e => e.GroupId == groupId && e.GroupNumber == groupNumber);
    }

    public Task<Message?> GetByGroupIdAndMessageId(Guid groupId, Guid messageId)
    {
        return Entities.SingleOrDefaultAsync(e => e.GroupId == groupId && e.Id == messageId);
    }

    public Task<List<Message>> GetAllMessagesInGroupAsync(Guid groupId)
    {
        return Entities.Where(e => e.GroupId == groupId).ToListAsync();
    }

    public Task<List<Message>> GetAllMessagesInGroupFromUserAsync(Guid groupId, Guid userId)
    {
        return Entities.Where(e => e.GroupId == groupId && e.UserId == userId).ToListAsync();
    }

    public Task<List<Message>> GetAllMessagesFromUserAsync(Guid userId)
    {
        return Entities.Where(e => e.UserId == userId).ToListAsync();
    }

    public Task<List<Message>> GetMessagesByGroupNum(Guid groupId, long? fromNum, long? toNum)
    {
        toNum ??= long.MaxValue;
        fromNum ??= 0;
        return Entities.Where(e => e.GroupId == groupId && e.GroupNumber >= fromNum && e.GroupNumber < toNum)
            .ToListAsync();
    }

    public Task<List<Message>> GetMessagesByDate(Guid groupId, DateTime? fromDate, DateTime? toDate)
    {
        toDate ??= DateTime.Now;
        return Entities.Where(e => e.GroupId == groupId && (fromDate == null || e.DateSent >= fromDate) && e.DateSent < toDate)
            .ToListAsync();
    }
}