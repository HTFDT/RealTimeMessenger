using Core.Base.Dal;

namespace Dal.Entities;

public class UserDal : BaseEntityDal<Guid>
{
    public required string Username { get; set; } = null!;
    public required string Password { get; set; } = null!;
    public ProfileDal? Profile { get; set; } = new();
    public RefreshTokenDal? RefreshToken { get; set; }
    public ICollection<UserRoleDal> UserRoles { get; set; } = null!;
}