using Core.Base.Dal;

namespace Domain.Entities;


/// <summary>
/// сущность прав, привязанных к ролям в группах (ManyToMany с GroupRoles)
/// </summary>
public class GroupRight : BaseEntityDal<Guid>
{
    public required string Name { get; set; }
    public string NormalizedName { get; set; } = null!;
    public required string Description { get; set; }

    public ICollection<GroupRole> GroupRoles { get; set; } = null!;
}