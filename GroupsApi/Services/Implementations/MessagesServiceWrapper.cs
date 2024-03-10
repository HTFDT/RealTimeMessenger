using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Services.Interfaces.Interfaces;
using Shared.Dto.Messages;
using Shared.Enums;

namespace Services.Implementations;

internal class MessagesServiceWrapper(IMessagesService messagesService, IRepositoryManager repoManager) : IMessagesServiceWrapper
{
    public async Task<IEnumerable<MessageResponse>> GetMessagesInGroup(Guid requesterId, Guid groupId, GetMessagesRequest request)
    {
        var member = await CheckMemberExists(groupId, requesterId);
        if (request.FilterBy == FilterMessagesBy.Date)
            return request.DateFrom is null || request.DateFrom < member.DateJoined
                ? await messagesService.GetMessagesInGroupBeforeJoin(requesterId, groupId, request.DateFrom,
                    request.DateTo)
                : await messagesService.GetMessagesInGroupAfterJoin(requesterId, groupId, request.DateFrom,
                    request.DateTo);
        
        return member.LastMessageNumberWhenJoined > 0 && 
               (request.GroupNumFrom is null || request.GroupNumFrom < member.LastMessageNumberWhenJoined)
            ? await messagesService.GetMessagesInGroupBeforeJoin(requesterId, groupId, request.GroupNumFrom,
                request.GroupNumTo)
            : await messagesService.GetMessagesInGroupAfterJoin(requesterId, groupId, request.GroupNumFrom,
                request.GroupNumTo);

    }

    public async Task<MessageResponse> GetMessageInGroup(Guid requesterId, Guid groupId, long groupNumber)
    {
        var member = await CheckMemberExists(groupId, requesterId);
        if (groupNumber < member.LastMessageNumberWhenJoined)
            return await messagesService.GetMessageInGroupBeforeJoin(requesterId, groupId, groupNumber);
        return await messagesService.GetMessageInGroupAfterJoin(requesterId, groupId, groupNumber);
    }

    public async Task<MessageResponse> GetMessageInGroup(Guid requesterId, Guid groupId, Guid messageId)
    {
        var member = await CheckMemberExists(groupId, requesterId);
        var message = await CheckMessageExists(groupId, messageId);
        if (message.GroupNumber < member.LastMessageNumberWhenJoined)
            return await messagesService.GetMessageInGroupBeforeJoin(requesterId, groupId, messageId);
        return await messagesService.GetMessageInGroupAfterJoin(requesterId, groupId, messageId);
    }

    public Task<MessageResponse> CreateMessage(Guid requesterId, Guid groupId, CreateMessageRequest request)
    {
        return messagesService.CreateMessage(requesterId, groupId, request);
    }

    public Task<MessageResponse> EditMessage(Guid requesterId, Guid groupId, long groupNumber, EditMessageRequest request)
    {
        return messagesService.EditMessage(requesterId, groupId, groupNumber, request);
    }

    public Task<MessageResponse> EditMessage(Guid requesterId, Guid groupId, Guid messageId, EditMessageRequest request)
    {
        return messagesService.EditMessage(requesterId, groupId, messageId, request);
    }

    public async Task DeleteMessage(Guid requesterId, Guid groupId, long groupNumber)
    {
        var member = await CheckMemberExists(groupId, requesterId);
        var message = await CheckMessageExists(groupId, groupNumber);
        if (message.MembershipId == member.Id)
            await messagesService.DeleteOwnMessage(requesterId, groupId, groupNumber);
        else
            await messagesService.DeleteSomeoneElseMessage(requesterId, groupId, groupNumber);
    }

    public async Task DeleteMessage(Guid requesterId, Guid groupId, Guid messageId)
    {
        var member = await CheckMemberExists(groupId, requesterId);
        var message = await CheckMessageExists(groupId, messageId);
        if (message.MembershipId == member.Id)
            await messagesService.DeleteOwnMessage(requesterId, groupId, messageId);
        else
            await messagesService.DeleteSomeoneElseMessage(requesterId, groupId, messageId);
    }

    public Task<MessageResponse> PinMessage(Guid requesterId, Guid groupId, Guid messageId)
    {
        return messagesService.PinMessage(requesterId, groupId, messageId);
    }

    public Task<MessageResponse> PinMessage(Guid requesterId, Guid groupId, long groupNumber)
    {
        return messagesService.PinMessage(requesterId, groupId, groupNumber);
    }

    public Task<MessageResponse> UnpinMessage(Guid requesterId, Guid groupId, Guid messageId)
    {
        return messagesService.UnpinMessage(requesterId, groupId, messageId);
    }

    public Task<MessageResponse> UnpinMessage(Guid requesterId, Guid groupId, long groupNumber)
    {
        return messagesService.UnpinMessage(requesterId, groupId, groupNumber);
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