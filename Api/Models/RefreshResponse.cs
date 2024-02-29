namespace Api.Models;

public class RefreshResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime AccessTokenExpiryDate { get; init; }
    public required DateTime RefreshTokenExpiryDate { get; init; }
}