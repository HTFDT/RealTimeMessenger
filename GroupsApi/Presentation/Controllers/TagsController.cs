using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Interfaces;
using Shared.Dto.Tags;

namespace Presentation.Controllers;


[ApiController]
[Authorize]
[Route("api/tags")]
public class TagsController(IServicesManager manager) : BaseGroupsApiController
{
    [HttpGet("{tagId:guid}")]
    [ProducesResponseType<TagResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTag([FromRoute] Guid tagId)
    {
        var response = await manager.Tags.GetTag(tagId);
        return Ok(response);
    }
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<TagResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTags()
    {
        var response = await manager.Tags.GetAllTags();
        return Ok(response);
    }
    
    [HttpPost]
    [Authorize(Policy = "Administrator")]
    [ProducesResponseType<TagResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagRequest request)
    {
        var response = await manager.Tags.CreateTag(request);
        return Ok(response);
    }
    
    [HttpPatch("{tagId:guid}")]
    [Authorize(Policy = "Administrator")]
    [ProducesResponseType<TagResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateTag([FromRoute] Guid tagId, [FromBody] UpdateTagRequest request)
    {
        var response = await manager.Tags.UpdateTag(tagId, request);
        return Ok(response);
    }
    
    [HttpDelete("{tagId:guid}")]
    [Authorize(Policy = "Administrator")]
    [ProducesResponseType<TagResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteTag([FromRoute] Guid tagId)
    {
        await manager.Tags.DeleteTag(tagId);
        return Ok();
    }
}