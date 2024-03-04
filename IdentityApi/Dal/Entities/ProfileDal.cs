using Core.Base.Dal;
using Dal.Enums;

namespace Dal.Entities;

public class ProfileDal : BaseEntityDal<Guid>
{
    public Gender? Gender { get; set; }
    public string? ProfileDescription { get; set; }

    public Guid UserId { get; set; }
    public UserDal User { get; set; } = null!;
}