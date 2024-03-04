using Core.Base.Dal;

namespace Dal.Entities;

public class RoleDal : BaseEntityDal<Guid>
{
    public required string Name { get; set; } = null!;
    public string NormalizedName { get; set; } = null!;
    public ICollection<UserRoleDal> UserRoles { get; set; } = null!;
}