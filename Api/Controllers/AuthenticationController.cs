using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Models;
using Logic;
using Logic.Models.User;
using Microsoft.AspNetCore.Authorization;
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!await userManager.CheckPasswordAsync(request.Username, request.Password))
            return Unauthorized();

        var user = await userManager.GetByUsername(request.Username);
        var accessToken = await jwtTokensManager.GenerateJwt(request.Username);
        var refreshToken = await jwtTokensManager.SetNewRefreshToken(user!.Id);

        return Ok(new LoginResponse
        {
            AccessToken = accessToken.AccessToken,
            AccessTokenExpiryDate = accessToken.ExpiryDate,
            RefreshToken = refreshToken.RefreshToken,
            RefreshTokenExpiryDate = refreshToken.ExpiryDate
        });
    }

    [HttpPost("refresh")]
    [ProducesResponseType<RefreshResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var principal = jwtTokensManager.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal.Identity?.Name is null)
            return Unauthorized();

        var user = await userManager.GetByUsername(principal.Identity.Name);
        if (user is null)
            return Unauthorized();
        if (await jwtTokensManager.IsRefreshTokenValid(user.Id, request.RefreshToken))
            return Unauthorized();
        
        var accessToken = await jwtTokensManager.GenerateJwt(user.Username);
        var newRefreshToken = await jwtTokensManager.SetNewRefreshToken(user.Id);
        
        return Ok(new LoginResponse
        {
            AccessToken = accessToken.AccessToken,
            AccessTokenExpiryDate = accessToken.ExpiryDate,
            RefreshToken = newRefreshToken.RefreshToken,
            RefreshTokenExpiryDate = newRefreshToken.ExpiryDate
        });
    }
    
    [Authorize]
    [HttpDelete("revoke")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Revoke()
    {
        if (User.Identity?.Name is null)
            return Unauthorized();

        var user = await userManager.GetByUsername(User.Identity.Name);
        if (user is null)
            return Unauthorized();

        await jwtTokensManager.RevokeRefreshToken(user.Id);
        return Ok();
    }
}