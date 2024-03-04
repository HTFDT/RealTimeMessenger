using Core.Base.Dal;

namespace Dal.Entities;

public class UserRoleDal : BaseEntityDal<Guid>
{
    public required Guid UserId { get; set; }
    public required Guid RoleId { get; set; }
    public UserDal User { get; set; } = null!;
    public RoleDal Role { get; set; } = null!;
}