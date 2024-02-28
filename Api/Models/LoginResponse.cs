namespace Api.Models;

public record LoginResponse
{
    public required string AccessToken { get; init; }
    public required DateTime ExpiryDate { get; set; }
}