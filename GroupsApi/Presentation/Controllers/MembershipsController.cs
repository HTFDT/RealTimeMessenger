using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Interfaces;
using Shared.Dto.Groups;
using Shared.Dto.Memberships;

namespace Presentation.Controllers;


[ApiController]
[Authorize]
[Route("api/groups/{groupId:guid}/members")]
public class MembershipsController(IServicesManager manager) : BaseGroupsApiController
{
    [HttpGet("{memberId:guid}")]
    [ProducesResponseType<MembershipResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMember([FromRoute] Guid groupId, [FromRoute] Guid memberId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Memberships.GetMember(requesterId, groupId, memberId);
        return Ok(response);
    }
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<MembershipResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMembers([FromRoute] Guid groupId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Memberships.GetAllMembers(requesterId, groupId);
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType<MembershipResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddMember([FromRoute] Guid groupId, [FromBody] AddMemberRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Memberships.AddMember(requesterId, groupId, request);
        return Ok(response);
    }
    
    [HttpPost("join")]
    [ProducesResponseType<MembershipResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> JoinGroup([FromRoute] Guid groupId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Memberships.JoinGroup(requesterId, groupId);
        return Ok(response);
    }
    
    [HttpPatch("leave")]
    [ProducesResponseType<MembershipResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LeaveGroup([FromRoute] Guid groupId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Memberships.LeaveGroup(requesterId, groupId);
        return Ok(response);
    }
    
    [HttpPatch("{memberId:guid}")]
    [ProducesResponseType<MembershipResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> KickMember([FromRoute] Guid groupId, [FromRoute] Guid memberId)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Memberships.KickMember(requesterId, groupId, memberId);
        return Ok(response);
    }
    
    [HttpPatch("{memberId:guid}/role")]
    [ProducesResponseType<MembershipResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AssignRoleToMember([FromRoute] Guid groupId, [FromRoute] Guid memberId, [FromBody] AssignRoleToMemberRequest request)
    {
        if (!TryGetUserIdFromClaims(out var requesterId))
            return Unauthorized(UnauthorizedMessage);
        var response = await manager.Memberships.AssignRoleToMember(requesterId, groupId, memberId, request);
        return Ok(response);
    }
}