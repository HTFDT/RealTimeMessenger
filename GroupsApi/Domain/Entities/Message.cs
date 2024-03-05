using Core.Base.Dal;

namespace Domain.Entities;


/// <summary>
/// сущность сообщений в группе (чате)
/// GroupId - внешний ключ на Group, (UserId, GroupId) - внешний ключ на UsersGroups
/// </summary>
public class Message : BaseEntityDal<Guid>
{
    public required Guid UserId { get; set; }
    public required Guid GroupId { get; set; }
    public required Guid MembershipId { get; set; }
    public long? GroupNumber { get; set; }
    public required DateTime DateSent { get; set; }
    public DateTime? DateEdited { get; set; }
    public Guid? ReplyTo { get; set; }
    public bool IsPinned { get; set; }
    public required string Text { get; set; }

    public Group Group { get; set; } = null!;
    public Membership Membership { get; set; } = null!;
}