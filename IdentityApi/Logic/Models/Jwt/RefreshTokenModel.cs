namespace Logic.Models.Jwt;

public record RefreshTokenModel
{
    public required string RefreshToken { get; init; }
    public required DateTime ExpiryDate { get; init; }
}