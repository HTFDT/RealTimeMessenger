namespace Logic.Models.Jwt;

public class JwtOptions
{
    public string ValidIssuer { get; set; } = null!;
    public string ValidAudience { get; set; } = null!;
    public int LifetimeInSeconds { get; set; }
    public int RefreshTokenLifetimeInDays { get; set; }
    public string Secret { get; set; } = null!;
}