using Core.Base.Dal;

namespace Domain.Entities;


/// <summary>
/// сущность групп (чатов)
/// </summary>
public class Group : BaseEntityDal<Guid>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required DateTime CreationDate { get; set; }
    public required bool IsPrivate { get; set; }
    public required Guid DefaultMemberRoleId { get; set; }
    public long LastMessageNum { get; set; }

    public ICollection<GroupRole> GroupRoles { get; set; } = null!;
    public ICollection<Tag> Tags { get; set; } = null!;
    public ICollection<Message> Messages { get; set; } = null!;
    public ICollection<Membership> Memberships { get; set; } = null!;
}