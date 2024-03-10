using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Services.Interfaces.Interfaces;
using Shared.Dto.Messages;
using Shared.Enums;

namespace Services.Implementations;

internal class MessagesService(IRepositoryManager repoManager) : IMessagesService
{
    public Task<IEnumerable<MessageResponse>> GetMessagesInGroupAfterJoin(Guid requesterId, Guid groupId, long? groupNumFrom, long? groupNumTo)
    {
        return GetMessagesInGroup(requesterId, groupId,
            m => m.GroupNumber >= (groupNumFrom ?? 0) && m.GroupNumber < (groupNumTo ?? long.MaxValue));
    }

    public Task<IEnumerable<MessageResponse>> GetMessagesInGroupAfterJoin(Guid requesterId, Guid groupId, DateTime? dateFrom, DateTime? dateTo)
    {
        return GetMessagesInGroup(requesterId, groupId,
            m => (dateFrom is null || m.DateSent >= dateFrom) && (dateTo is null || m.DateSent < dateTo));
    }

    public Task<IEnumerable<MessageResponse>> GetMessagesInGroupBeforeJoin(Guid requesterId, Guid groupId, long? groupNumFrom, long? groupNumTo)
    {
        return GetMessagesInGroup(requesterId, groupId,
            m => m.GroupNumber >= (groupNumFrom ?? 0) && m.GroupNumber < (groupNumTo ?? long.MaxValue));
    }

    public Task<IEnumerable<MessageResponse>> GetMessagesInGroupBeforeJoin(Guid requesterId, Guid groupId, DateTime? dateFrom, DateTime? dateTo)
    {
        return GetMessagesInGroup(requesterId, groupId,
            m => (dateFrom is null || m.DateSent >= dateFrom) && (dateTo is null || m.DateSent < dateTo));
    }
    
    private async Task<IEnumerable<MessageResponse>> GetMessagesInGroup(Guid requesterId, Guid groupId, Func<Message, bool> predicate)
    {
        var group = await CheckGroupExists(groupId);
        await CheckMemberExists(groupId, requesterId);
        await repoManager.Groups.LoadCollection(group, g => g.Messages);
        return group.Messages.Where(predicate).Select(m => new MessageResponse(m.Id,
            m.MembershipId,
            m.GroupId,
            m.GroupNumber,
            m.DateSent,
            m.DateEdited,
            m.ReplyTo,
            m.IsPinned,
            m.Text));
    }

    public Task<MessageResponse> GetMessageInGroupBeforeJoin(Guid requesterId, Guid groupId, Guid messageId)
    {
        return GetMessageInGroup(requesterId, groupId, m => m.Id == messageId,
            new MessageNotFoundException(groupId, messageId));
    }

    public Task<MessageResponse> GetMessageInGroupBeforeJoin(Guid requesterId, Guid groupId, long groupNum)
    {
        return GetMessageInGroup(requesterId, groupId, m => m.GroupNumber == groupNum,
            new MessageNotFoundException(groupId, groupNum));
    }

    public Task<MessageResponse> GetMessageInGroupAfterJoin(Guid requesterId, Guid groupId, Guid messageId)
    {
        return GetMessageInGroup(requesterId, groupId, m => m.Id == messageId,
            new MessageNotFoundException(groupId, messageId));
    }

    public Task<MessageResponse> GetMessageInGroupAfterJoin(Guid requesterId, Guid groupId, long groupNum)
    {
        return GetMessageInGroup(requesterId, groupId, m => m.GroupNumber == groupNum,
            new MessageNotFoundException(groupId, groupNum));
    }

    private async Task<MessageResponse> GetMessageInGroup(Guid requesterId, 
        Guid groupId,
        Func<Message, bool> predicate,
        Exception exception)
    {
        var group = await CheckGroupExists(groupId);
        await CheckMemberExists(groupId, requesterId);
        // загружаем коллекцию сообщений группы
        await repoManager.Groups.LoadCollection(group, g => g.Messages);
        // проверяем, что сообщение существует
        var message = group.Messages.SingleOrDefault(predicate);
        if (message is null)
            throw exception;
        
        return new MessageResponse(message.Id,
            message.MembershipId,
            message.GroupId,
            message.GroupNumber,
            message.DateSent,
            message.DateEdited,
            message.ReplyTo,
            message.IsPinned,
            message.Text);
    }

    public async Task<MessageResponse> CreateMessage(Guid requesterId, Guid groupId, CreateMessageRequest request)
    {
        var group = await CheckGroupExists(groupId);
        var member = await CheckMemberExists(groupId, requesterId);
        var message = new Message
        {
            UserId = requesterId,
            GroupId = groupId,
            MembershipId = member.Id,
            DateSent = DateTime.UtcNow,
            ReplyTo = request.ReplyTo,
            Text = request.Text
        };
        await repoManager.Messages.CreateAsync(message);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new MessageResponse(message.Id,
            message.MembershipId,
            message.GroupId,
            message.GroupNumber,
            message.DateSent,
            message.DateEdited,
            message.ReplyTo,
            message.IsPinned,
            message.Text);
    }

    public Task<MessageResponse> EditMessage(Guid requesterId, Guid groupId, long groupNumber, EditMessageRequest request)
    {
        return EditMessage(requesterId,
            groupId,
            m => m.GroupNumber == groupNumber,
            new EditNotOwnMessageForbiddenException(groupNumber),
            request);
    }

    public Task<MessageResponse> EditMessage(Guid requesterId, Guid groupId, Guid messageId, EditMessageRequest request)
    {
        return EditMessage(requesterId,
            groupId,
            m => m.Id == messageId,
            new EditNotOwnMessageForbiddenException(messageId),
            request);
    }

    private async Task<MessageResponse> EditMessage(Guid requesterId,
        Guid groupId,
        Func<Message, bool> predicate,
        Exception exception,
        EditMessageRequest request)
    {
        var group = await CheckGroupExists(groupId);
        var member = await CheckMemberExists(groupId, requesterId);
        await repoManager.Memberships.LoadCollection(member, m => m.Messages);
        var message = member.Messages.SingleOrDefault(predicate);
        if (message is null)
            throw exception;
        message.Text = request.Text;
        message.DateEdited = DateTime.UtcNow;
        await repoManager.Messages.UpdateAsync(message);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new MessageResponse(message.Id,
            message.MembershipId,
            message.GroupId,
            message.GroupNumber,
            message.DateSent,
            message.DateEdited,
            message.ReplyTo,
            message.IsPinned,
            message.Text);
    }
    
    public Task DeleteOwnMessage(Guid requesterId, Guid groupId, long groupNumber)
    {
        return DeleteMessage(requesterId, groupId, m => m.GroupNumber == groupNumber, new MessageNotFoundException(groupId, groupNumber));
    }

    public Task DeleteOwnMessage(Guid requesterId, Guid groupId, Guid messageId)
    {
        return DeleteMessage(requesterId, groupId, m => m.Id == messageId, new MessageNotFoundException(groupId, messageId));
    }

    public Task DeleteSomeoneElseMessage(Guid requesterId, Guid groupId, long groupNumber)
    {
        return DeleteMessage(requesterId, groupId, m => m.GroupNumber == groupNumber, new MessageNotFoundException(groupId, groupNumber));
    }

    public Task DeleteSomeoneElseMessage(Guid requesterId, Guid groupId, Guid messageId)
    {
        return DeleteMessage(requesterId, groupId, m => m.Id == messageId, new MessageNotFoundException(groupId, messageId));
    }
    
    public async Task<MessageResponse> PinMessage(Guid requesterId, Guid groupId, Guid messageId)
    {
        var group = await CheckGroupExists(groupId);
        var message = await CheckMessageExists(group.Id, messageId);
        message.IsPinned = true;
        await repoManager.Messages.UpdateAsync(message);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new MessageResponse(message.Id,
            message.MembershipId,
            message.GroupId,
            message.GroupNumber,
            message.DateSent,
            message.DateEdited,
            message.ReplyTo,
            message.IsPinned,
            message.Text); 
    }
    
    public async Task<MessageResponse> PinMessage(Guid requesterId, Guid groupId, long groupNumber)
    {
        var group = await CheckGroupExists(groupId);
        var message = await CheckMessageExists(group.Id, groupNumber);
        message.IsPinned = true;
        await repoManager.Messages.UpdateAsync(message);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new MessageResponse(message.Id,
            message.MembershipId,
            message.GroupId,
            message.GroupNumber,
            message.DateSent,
            message.DateEdited,
            message.ReplyTo,
            message.IsPinned,
            message.Text); 
    }
    
    public async Task<MessageResponse> UnpinMessage(Guid requesterId, Guid groupId, Guid messageId)
    {
        var group = await CheckGroupExists(groupId);
        var message = await CheckMessageExists(group.Id, messageId);
        message.IsPinned = false;
        await repoManager.Messages.UpdateAsync(message);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new MessageResponse(message.Id,
            message.MembershipId,
            message.GroupId,
            message.GroupNumber,
            message.DateSent,
            message.DateEdited,
            message.ReplyTo,
            message.IsPinned,
            message.Text); 
    }
    
    public async Task<MessageResponse> UnpinMessage(Guid requesterId, Guid groupId, long groupNumber)
    {
        var group = await CheckGroupExists(groupId);
        var message = await CheckMessageExists(group.Id, groupNumber);
        message.IsPinned = false;
        await repoManager.Messages.UpdateAsync(message);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new MessageResponse(message.Id,
            message.MembershipId,
            message.GroupId,
            message.GroupNumber,
            message.DateSent,
            message.DateEdited,
            message.ReplyTo,
            message.IsPinned,
            message.Text); 
    }

    private async Task DeleteMessage(Guid requesterId, Guid groupId, Func<Message, bool> predicate, Exception exception)
    {
        var group = await CheckGroupExists(groupId);
        await CheckMemberExists(groupId, requesterId);
        await repoManager.Groups.LoadCollection(group, g => g.Messages);
        var message = group.Messages.SingleOrDefault(predicate);
        if (message is null)
            throw exception;
        await repoManager.Messages.DeleteAsync(message);
        await repoManager.UnitOfWork.SaveChangesAsync();
    }

    private async Task<Group> CheckGroupExists(Guid groupId)
    {
        var group = await repoManager.Groups.GetByIdAsync(groupId);
        if (group is null)
            throw new GroupNotFoundException(groupId);
        return group;
    }

    private async Task<Membership> CheckMemberExists(Guid groupId, Guid userId)
    {
        var member = await repoManager.Memberships.GetByUserIdAndGroupId(userId, groupId);
        if (member is null)
            throw new MemberNotFoundException(userId, groupId);
        return member;
    }
    
    private async Task<Message> CheckMessageExists(Guid groupId, Guid messageId)
    {
        var message = await repoManager.Messages.GetByGroupIdAndMessageId(groupId, messageId);
        if (message is null)
            throw new MessageNotFoundException(groupId, messageId);
        return message;
    }
    
    private async Task<Message> CheckMessageExists(Guid groupId, long groupNumber)
    {
        var message = await repoManager.Messages.GetByGroupIdAndGroupNumber(groupId, groupNumber);
        if (message is null)
            throw new MessageNotFoundException(groupId, groupNumber);
        return message;
    }
}