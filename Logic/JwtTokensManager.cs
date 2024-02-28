using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dal.Repository;
using Dal.Repository.Interfaces;
using Logic.Models.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Logic;

public class JwtTokensManager(JwtOptions jwtOptions, IUserRepository userRepository)
{
    public async Task<AccessTokenModel> GenerateJwt(string username)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user is null)
            throw new InvalidOperationException($"no such user with username '{username}'");
        var roles = await userRepository.GetUserRolesAsync(user.Id);

        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        authClaims.AddRange(roleClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            jwtOptions.Secret ?? throw new InvalidOperationException("Secret not configured")));

        var token = new JwtSecurityToken(
            issuer: jwtOptions.ValidIssuer,
            audience: jwtOptions.ValidAudience,
            expires: DateTime.UtcNow.AddSeconds(jwtOptions.LifetimeInSeconds),
            claims: authClaims,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new AccessTokenModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiryDate = token.ValidTo
        };
    }
}