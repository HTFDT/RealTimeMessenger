using Core.Extensions;
using Core.MassTransit.Messages;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace web.Controllers;

[ApiController]
[Route("api")]
public class MainController(IRequestClient<ProcessMembership> client) : Controller
{
    [Authorize]
    [ProducesResponseType<MembershipResponse>(StatusCodes.Status200OK)]
    [HttpGet("groups/{groupId:guid}/memberships/{memberId:guid}")]
    public async Task<IActionResult> GetGroupMemberships(Guid groupId, Guid memberId)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();
        // запрос сущности, объединяющей данные из двух сервисов
        var response = await client.GetResponse<MembershipWithUsernameResponse>(new
        {
            RequesterId = userId,
            MembershipId = memberId,
            GroupId = groupId
        });
        return Ok(response);
    }
}