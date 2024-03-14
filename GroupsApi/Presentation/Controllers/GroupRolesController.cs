using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Interfaces;
using Shared.Dto.GroupRoles;

namespace Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/groups/{groupId:guid}/roles")]
public class GroupRolesController(IServicesManager manager) : BaseGroupsApiController
{
    [Authorize(Policy = "Administrator")]
    [HttpGet("~/api/default-roles")]
    [ProducesResponseType<IEnumerable<GroupRoleResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDefaultGroupRoles()
    {
        var response = await manager.GroupRoles.GetDefaultGroupRoles();
        return Ok(response);
    }
    
    [Authorize(Policy = "Administrator")]
    [HttpPatch("~/api/default-roles/{id:guid}")]
    [ProducesResponseType<GroupRoleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateDefaultGroupRole([FromRoute] Guid id, [FromBody] UpdateGroupRoleRequest request)
    {
        var response = await manager.GroupRoles.UpdateDefaultGroupRole(id, request);
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType<IEnumerable<GroupRoleResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateGroupRole([FromRoute] Guid groupId, [FromBody] CreateGroupRoleRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.GroupRoles.CreateGroupRole(requesterId, groupId, request);
        return Ok(response);
    }
    
    [HttpPatch("{roleId:guid}")]
    [ProducesResponseType<GroupRoleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateGroupRole([FromRoute] Guid groupId, [FromRoute] Guid roleId, [FromBody] UpdateGroupRoleRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.GroupRoles.UpdateGroupRole(requesterId, groupId, roleId, request);
        return Ok(response);
    }
    
    [HttpGet("{roleId:guid}")]
    [ProducesResponseType<GroupRoleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGroupRole([FromRoute] Guid groupId, [FromRoute] Guid roleId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.GroupRoles.GetGroupRole(requesterId, groupId, roleId);
        return Ok(response);
    }
    
    [HttpGet]
    [ProducesResponseType<GroupRoleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGroupRoles([FromRoute] Guid groupId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.GroupRoles.GetGroupRoles(requesterId, groupId);
        return Ok(response);
    }
    
    [HttpDelete("{roleId:guid}")]
    [ProducesResponseType<GroupRoleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteGroupRole([FromRoute] Guid groupId, [FromRoute] Guid roleId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        await manager.GroupRoles.DeleteGroupRole(requesterId, groupId, roleId);
        return Ok();
    }
    
    [HttpPost("{roleId:guid}/rights")]
    [ProducesResponseType<GroupRoleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddRightToGroupRole([FromRoute] Guid groupId, [FromRoute] Guid roleId, [FromBody] AddRightToGroupRoleRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        await manager.GroupRoles.AddRightToGroupRole(requesterId, groupId, roleId, request);
        return Ok();
    }
    
    [HttpDelete("{roleId:guid}/rights/{rightId:guid}")]
    [ProducesResponseType<GroupRoleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveRightFromGroupRole([FromRoute] Guid groupId, [FromRoute] Guid roleId, [FromRoute] Guid rightId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        await manager.GroupRoles.RemoveRightFromGroupRole(requesterId, groupId, roleId, rightId);
        return Ok();
    }
}