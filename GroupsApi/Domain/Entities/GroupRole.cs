using Core.Base.Dal;

namespace Domain.Entities;


/// <summary>
/// сущность внутричатовых ролей, пользователь со специальной ролью может их создавать/редактировать их права/удалять
/// (ManyToMany с GroupRights)
/// </summary>
public class GroupRole : BaseEntityDal<Guid>
{
    public required bool IsRevocable { get; set; }
    // если внутри группы уже есть участник с этой ролью, то выдать её другому участнику нельзя
    public required bool IsUnique { get; set; }
    public bool IsDefault { get; set; }
    public required bool IsAssignableByUsers { get; set; }
    public bool IsDefaultMemberRole { get; set; }
    public bool IsDefaultKickedRole { get; set; }
    public bool IsDefaultOwnerRole { get; set; }
    public bool IsDefaultLeftRole { get; set; }
    public required Guid? GroupId { get; set; }
    public required string Name { get; set; }
    public string NormalizedName { get; set; } = null!;
    public required string Description { get; set; }

    public Group? Group { get; set; }
    public ICollection<Membership> Memberships { get; set; } = null!;
    public ICollection<Membership> MembershipsWithThisRoleBeforeLeave { get; set; } = null!;
    public ICollection<GroupRight> GroupRights { get; set; } = null!;
}