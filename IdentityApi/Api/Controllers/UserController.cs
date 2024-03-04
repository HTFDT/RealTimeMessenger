using Api.Models;
using Logic;
using Logic.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("users")]
public class UserController(UserManager userManager) : Controller
{
    [Authorize(Policy = "Superuser")]
    [HttpPut("{id}/role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignRole(Guid id, [FromBody] AssignRoleRequest request)
    {
        try
        {
            await userManager.AssignUserRole(id, request.RoleName);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize(Policy = "Superuser")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await userManager.DeleteUser(id);
        return Ok();
    }

    [Authorize(Policy = "Administrator")]
    [HttpGet]
    [ProducesResponseType<IEnumerable<GetUserResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userManager.GetAllUsers();
        return Ok(users.Select(u => new GetUserResponse
        {
            Id = u.Id,
            Username = u.Username
        }));
    }
    
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType<GetUserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await userManager.GetById(id);
        if (user is null)
            return NotFound();
        
        return Ok(new GetUserResponse
        {
            Id = user.Id,
            Username = user.Username
        });
    }
    
    [Authorize]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SetUserProfile([FromBody] SetProfileRequest request)
    {
        if (User.Identity?.Name is null)
            return Unauthorized();

        var user = await userManager.GetByUsername(User.Identity.Name);
        if (user is null)
            return Unauthorized();

        var res = await userManager.SetUserProfile(user.Id, new ProfileModel
        {
            ProfileDescription = request.ProfileDescription,
            Gender = request.Gender
        });
        return res ? Ok() : StatusCode(500);
    }
}