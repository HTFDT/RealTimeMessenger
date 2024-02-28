using Api.Models;
using Logic;
using Logic.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController(UserManager userManager) : Controller
{
    [HttpPost("register")]
    [ProducesResponseType<RegisterResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = await userManager.GetByUsername(request.Username);
        if (user is not null)
            return Conflict($"User with username '{request.Username}' already exists");
        var id = await userManager.CreateUser(new UserInModel
        {
            Username = request.Username,
            Password = request.Password
        });
        return Ok(new RegisterResponse
        {
            Id = id
        });
    }
}