using Domain.Interfaces;
using Services.Interfaces.Interfaces;
using Shared.Dto.Messages;

namespace Services.Decorators;

internal class MessagesServiceEnsureRightsDecorator(IMessagesService decoratee, IRepositoryManager repoManager) 
    : BaseEnsureRightsDecorator(repoManager), IMessagesService
{
    public async Task<IEnumerable<MessageResponse>> GetMessagesInGroupAfterJoin(Guid requesterId, Guid groupId, long? groupNumFrom, long? groupNumTo)
    {
        await EnsureRights(decoratee, 
            "GetMessagesInGroupAfterJoin", 
            requesterId,
            groupId,
            typeof(long?),
            typeof(long?));
        
        return await decoratee.GetMessagesInGroupAfterJoin(requesterId, groupId, groupNumFrom, groupNumTo);
    }

    public async Task<IEnumerable<MessageResponse>> GetMessagesInGroupAfterJoin(Guid requesterId, Guid groupId, DateTime? dateFrom, DateTime? dateTo)
    {
        await EnsureRights(decoratee, 
            "GetMessagesInGroupAfterJoin", 
            requesterId,
            groupId,
            typeof(DateTime?), 
            typeof(DateTime?));
        
        return await decoratee.GetMessagesInGroupAfterJoin(requesterId, groupId, dateFrom, dateTo);
    }

    public async Task<IEnumerable<MessageResponse>> GetMessagesInGroupBeforeJoin(Guid requesterId, Guid groupId, long? groupNumFrom, long? groupNumTo)
    {
        await EnsureRights(decoratee, 
            "GetMessagesInGroupBeforeJoin", 
            requesterId,
            groupId,
            typeof(long?),
            typeof(long?));
        
        return await decoratee.GetMessagesInGroupAfterJoin(requesterId, groupId, groupNumFrom, groupNumTo);
    }

    public async Task<IEnumerable<MessageResponse>> GetMessagesInGroupBeforeJoin(Guid requesterId, Guid groupId, DateTime? dateFrom, DateTime? dateTo)
    {
        await EnsureRights(decoratee, 
            "GetMessagesInGroupBeforeJoin", 
            requesterId,
            groupId,
            typeof(DateTime?), 
            typeof(DateTime?));
        
        return await decoratee.GetMessagesInGroupBeforeJoin(requesterId, groupId, dateFrom, dateTo);
    }

    public async Task<MessageResponse> GetMessageInGroupBeforeJoin(Guid requesterId, Guid groupId, Guid messageId)
    {
        await EnsureRights(decoratee, 
            "GetMessageInGroupBeforeJoin", 
            requesterId,
            groupId,
            messageId.GetType());
        
        return await decoratee.GetMessageInGroupBeforeJoin(requesterId, groupId, messageId);
    }

    public async Task<MessageResponse> GetMessageInGroupBeforeJoin(Guid requesterId, Guid groupId, long groupNum)
    {
        await EnsureRights(decoratee, 
            "GetMessageInGroupBeforeJoin", 
            requesterId,
            groupId,
            groupNum.GetType());
        
        return await decoratee.GetMessageInGroupBeforeJoin(requesterId, groupId, groupNum);
    }

    public async Task<MessageResponse> GetMessageInGroupAfterJoin(Guid requesterId, Guid groupId, Guid messageId)
    {
        await EnsureRights(decoratee, 
            "GetMessageInGroupAfterJoin", 
            requesterId,
            groupId,
            messageId.GetType());
        
        return await decoratee.GetMessageInGroupAfterJoin(requesterId, groupId, messageId);
    }

    public async Task<MessageResponse> GetMessageInGroupAfterJoin(Guid requesterId, Guid groupId, long groupNum)
    {
        await EnsureRights(decoratee, 
            "GetMessageInGroupAfterJoin", 
            requesterId,
            groupId,
            groupNum.GetType());
        
        return await decoratee.GetMessageInGroupAfterJoin(requesterId, groupId, groupNum);
    }

    public async Task<MessageResponse> CreateMessage(Guid requesterId, Guid groupId, CreateMessageRequest request)
    {
        await EnsureRights(decoratee, 
            "CreateMessage", 
            requesterId,
            groupId,
            request.GetType());
        
        return await decoratee.CreateMessage(requesterId, groupId, request);
    }

    public async Task<MessageResponse> EditMessage(Guid requesterId, Guid groupId, long groupNumber, EditMessageRequest request)
    {
        return await decoratee.EditMessage(requesterId, groupId, groupNumber, request);
    }

    public async Task<MessageResponse> EditMessage(Guid requesterId, Guid groupId, Guid messageId, EditMessageRequest request)
    {
        return await decoratee.EditMessage(requesterId, groupId, messageId, request);
    }

    public async Task DeleteOwnMessage(Guid requesterId, Guid groupId, long groupNumber)
    {
        await decoratee.DeleteOwnMessage(requesterId, groupId, groupNumber);
    }

    public async Task DeleteOwnMessage(Guid requesterId, Guid groupId, Guid messageId)
    {
        await decoratee.DeleteOwnMessage(requesterId, groupId, messageId);
    }

    public async Task DeleteSomeoneElseMessage(Guid requesterId, Guid groupId, long groupNumber)
    {
        await EnsureRights(decoratee, 
            "DeleteSomeoneElseMessage", 
            requesterId,
            groupId,
            groupNumber.GetType());
        
        await decoratee.DeleteSomeoneElseMessage(requesterId, groupId, groupNumber);
    }

    public async Task DeleteSomeoneElseMessage(Guid requesterId, Guid groupId, Guid messageId)
    {
        await EnsureRights(decoratee, 
            "DeleteSomeoneElseMessage", 
            requesterId,
            groupId,
            messageId.GetType());
        
        await decoratee.DeleteSomeoneElseMessage(requesterId, groupId, messageId);
    }

    public async Task<MessageResponse> PinMessage(Guid requesterId, Guid groupId, Guid messageId)
    {
        await EnsureRights(decoratee, 
            "PinMessage", 
            requesterId,
            groupId,
            messageId.GetType());
        
        return await decoratee.PinMessage(requesterId, groupId, messageId);
    }

    public async Task<MessageResponse> PinMessage(Guid requesterId, Guid groupId, long groupNumber)
    {
        await EnsureRights(decoratee, 
            "PinMessage", 
            requesterId,
            groupId,
            groupNumber.GetType());
        
        return await decoratee.PinMessage(requesterId, groupId, groupNumber);
    }

    public async Task<MessageResponse> UnpinMessage(Guid requesterId, Guid groupId, Guid messageId)
    {
        await EnsureRights(decoratee, 
            "UnpinMessage", 
            requesterId,
            groupId,
            messageId.GetType());
        
        return await decoratee.UnpinMessage(requesterId, groupId, messageId);
    }

    public async Task<MessageResponse> UnpinMessage(Guid requesterId, Guid groupId, long groupNumber)
    {
        await EnsureRights(decoratee, 
            "UnpinMessage", 
            requesterId,
            groupId,
            groupNumber.GetType());
        
        return await decoratee.UnpinMessage(requesterId, groupId, groupNumber);
    }
}