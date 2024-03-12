using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Interfaces;
using Shared.Dto.Groups;

namespace Presentation.Controllers;

[ApiController]
[Route("api/groups")]
[Authorize]
public class GroupsController(IServicesManager manager) : BaseGroupsApiController
{
    [HttpPost]
    [ProducesResponseType<GroupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Groups.CreateGroup(requesterId, request);
        return Ok(response);
    }
    
    [HttpPatch("{id:guid}")]
    [ProducesResponseType<GroupResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateGroup([FromRoute] Guid id, [FromBody] UpdateGroupRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Groups.UpdateGroup(requesterId, id, request);
        return Ok(response);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType<GroupResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroup([FromRoute] Guid id)
    {
        var response = await manager.Groups.GetGroup(id);
        return Ok(response);
    }
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<GroupResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllGroups()
    {
        var response = await manager.Groups.GetAllGroups();
        return Ok(response);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteGroup([FromRoute] Guid id)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        await manager.Groups.DeleteGroup(requesterId, id);
        return Ok();
    }
    
    [HttpPost("{groupId:guid}/tags")]
    [ProducesResponseType<GroupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddTag([FromRoute] Guid groupId, [FromBody] AddTagRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        await manager.Groups.AddTag(requesterId, groupId, request);
        return Ok();
    }
    
    [HttpDelete("{groupId:guid}/tags/{tagId:guid}")]
    [ProducesResponseType<GroupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveTag([FromRoute] Guid groupId, [FromRoute] Guid tagId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        await manager.Groups.RemoveTag(requesterId, groupId, tagId);
        return Ok();
    }
    
    [HttpPatch("{groupId:guid}/roles/default-member-role")]
    [ProducesResponseType<GroupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SetDefaultMemberRole([FromRoute] Guid groupId, SetDefaultMemberRoleRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Groups.SetDefaultMemberRole(requesterId, groupId, request);
        return Ok(response);
    }
}