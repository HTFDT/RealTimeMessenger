using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Interfaces;
using Shared.Dto.GroupRights;

namespace Presentation.Controllers;


[ApiController]
[Authorize]
[Route("api/rights")]
public class GroupRightsController(IServicesManager manager) : BaseGroupsApiController
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<GroupRightResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllGroupRights()
    {
        var response = await manager.GroupRights.GetAllGroupRights();
        return Ok(response);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType<IEnumerable<GroupRightResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGroupRight([FromRoute] Guid id)
    {
        var response = await manager.GroupRights.GetGroupRight(id);
        return Ok(response);
    }
}