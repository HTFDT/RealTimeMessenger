using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Interfaces;
using Shared.Dto.Messages;

namespace Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/groups/{groupId:guid}/messages")]
public class MessagesController(IServicesManager manager) : BaseGroupsApiController
{
    [HttpPost]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateMessage([FromRoute] Guid groupId, [FromBody] CreateMessageRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Messages.CreateMessage(requesterId, groupId, request);
        return Ok(response);
    }
    
    [HttpPatch("{messageId:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> EditMessage([FromRoute] Guid groupId, [FromRoute] Guid messageId, [FromBody] EditMessageRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Messages.EditMessage(requesterId, groupId, messageId, request);
        return Ok(response);
    }
    
    [HttpPatch("{groupNumber:long}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> EditMessage([FromRoute] Guid groupId, [FromRoute] long groupNumber, [FromBody] EditMessageRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Messages.EditMessage(requesterId, groupId, groupNumber, request);
        return Ok(response);
    }
    
    [HttpGet("{messageId:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessage([FromRoute] Guid groupId, [FromRoute] Guid messageId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Messages.GetMessageInGroup(requesterId, groupId, messageId);
        return Ok(response);
    }
    
    [HttpGet("{groupNumber:long}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessage([FromRoute] Guid groupId, [FromRoute] long groupNumber)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Messages.GetMessageInGroup(requesterId, groupId, groupNumber);
        return Ok(response);
    }
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<MessageResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMessages([FromRoute] Guid groupId, [FromQuery] GetMessagesRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Messages.GetMessagesInGroup(requesterId, groupId, request);
        return Ok(response);
    }
    
    [HttpDelete("{messageId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteMessage([FromRoute] Guid groupId, [FromRoute] Guid messageId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        await manager.Messages.DeleteMessage(requesterId, groupId, messageId);
        return Ok();
    }
    
    [HttpDelete("{groupNumber:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteMessage([FromRoute] Guid groupId, [FromRoute] long groupNumber)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        await manager.Messages.DeleteMessage(requesterId, groupId, groupNumber);
        return Ok();
    }
    
    [HttpPatch("{messageId:guid}/pin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PinMessage([FromRoute] Guid groupId, [FromRoute] Guid messageId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        await manager.Messages.PinMessage(requesterId, groupId, messageId);
        return Ok();
    }
    
    [HttpPatch("{groupNumber:long}/pin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PinMessage([FromRoute] Guid groupId, [FromRoute] long groupNumber)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        await manager.Messages.PinMessage(requesterId, groupId, groupNumber);
        return Ok();
    }
    
    [HttpPatch("{messageId:guid}/unpin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UnpinMessage([FromRoute] Guid groupId, [FromRoute] Guid messageId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        await manager.Messages.UnpinMessage(requesterId, groupId, messageId);
        return Ok();
    }
    
    [HttpPatch("{groupNumber:long}/unpin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UnpinMessage([FromRoute] Guid groupId, [FromRoute] long groupNumber)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        await manager.Messages.UnpinMessage(requesterId, groupId, groupNumber);
        return Ok();
    }
}