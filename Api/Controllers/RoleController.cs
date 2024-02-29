using Api.Models;
using Logic;
using Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("roles")]
public class RoleController(RoleManager roleManager) : Controller
{
    [Authorize(Policy = "Superuser")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        await roleManager.CreateRole(new RoleInModel
        {
            RoleName = request.RoleName
        });

        return Created();
    }
    
    [Authorize(Policy = "Superuser")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        await roleManager.DeleteRole(id);
        return Ok();
    }
    
    
    [Authorize(Policy = "Administrator")]
    [HttpGet]
    [ProducesResponseType<IEnumerable<RoleResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await roleManager.GetAllRoles();
        return Ok(roles.Select(r => new RoleResponse
        {
            Id = r.Id,
            Name = r.RoleName
        }));
    }
}