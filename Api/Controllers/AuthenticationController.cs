using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Models;
using Logic;
using Logic.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController(UserManager userManager, JwtTokensManager jwtTokensManager) : Controller
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

    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!await userManager.CheckPasswordAsync(request.Username, request.Password))
            return Unauthorized();

        var accessToken = await jwtTokensManager.GenerateJwt(request.Username);

        return Ok(new LoginResponse
        {
            AccessToken = accessToken.AccessToken,
            ExpiryDate = accessToken.ExpiryDate
        });
    }
}