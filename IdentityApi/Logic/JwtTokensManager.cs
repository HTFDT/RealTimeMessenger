using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.Base.Jwt;
using Dal.Entities;
using Dal.Repository;
using Dal.Repository.Interfaces;
using Logic.Models.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Logic;

public class JwtTokensManager(JwtOptions jwtOptions, IUserRepository userRepository)
{
    public async Task<RefreshTokenModel> SetNewRefreshToken(Guid userId)
    {
        var newToken = GenerateRefreshToken();
        var newExpiryDate = DateTime.UtcNow.AddDays(jwtOptions.RefreshTokenLifetimeInDays);
       
        await userRepository.SetUserRefreshTokenAsync(userId, newToken, newExpiryDate);
        
        return new RefreshTokenModel
        {
            RefreshToken = newToken,
            ExpiryDate = newExpiryDate
        };
    }

    public async Task<RefreshTokenModel?> GetRefreshToken(Guid userId)
    {
        var token = await userRepository.GetUserRefreshTokenAsync(userId);
        if (token is null)
            return null;
        return new RefreshTokenModel
        {
            RefreshToken = token.RefreshToken,
            ExpiryDate = token.ExpiryDate!.Value
        };
    }

    public Task RevokeRefreshToken(Guid userId)
    {
        return userRepository.RevokeUserRefreshTokenAsync(userId);
    }
    
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        using var generator = RandomNumberGenerator.Create();

        generator.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var secret = jwtOptions.Secret ?? throw new InvalidOperationException("Secret not configured");

        var validation = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }
    
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
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        authClaims.AddRange(roleClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            jwtOptions.Secret ?? throw new InvalidOperationException("Secret not configured")));

        var token = new JwtSecurityToken(
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

    public async Task<bool> IsRefreshTokenValid(Guid userId, string requestToken)
    {
        var refreshToken = await GetRefreshToken(userId);
        return refreshToken is null || refreshToken.RefreshToken != requestToken
                                    || DateTime.UtcNow > refreshToken.ExpiryDate;
    }
}