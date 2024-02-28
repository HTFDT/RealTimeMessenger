namespace Logic.Models.Jwt;

public record AccessTokenModel
{
    public required string AccessToken { get; init; }
    public required DateTime ExpiryDate { get; init; }
}