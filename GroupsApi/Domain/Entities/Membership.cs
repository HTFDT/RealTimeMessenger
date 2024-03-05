using Core.Base.Dal;

namespace Domain.Entities;


/// <summary>
/// сущность-связка пользователей приложения и групп, в которых они состоят (ManyToMany)
/// </summary>
public class Membership : BaseEntityDal<Guid>
{
    public required Guid UserId { get; set; }
    public required Guid GroupId { get; set; }
    public required Guid? GroupRoleId { get; set; }
    public required bool IsRoleUnique { get; set; }
    public required DateTime DateJoined { get; set; }
    public required long LastMessageNumberWhenJoined { get; set; }
    public required bool IsKicked { get; set; }
    public required bool IsLeft { get; set; }
    public Guid? GroupRoleBeforeLeaveId { get; set; }
    public required bool IsOwner { get; set; }
    public DateTime? DateKicked { get; set; }

    public GroupRole GroupRole { get; set; } = null!;
    public GroupRole? GroupRoleBeforeLeave { get; set; }
    public Group Group { get; set; } = null!;
    public ICollection<Message> Messages { get; set; } = null!;
}