namespace Core.Jwt;

public class JwtOptions
{
    public int LifetimeInSeconds { get; set; }
    public int RefreshTokenLifetimeInDays { get; set; }
    public string Secret { get; set; } = null!;
}