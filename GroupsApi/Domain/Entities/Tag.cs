using Core.Base.Dal;

namespace Domain.Entities;


/// <summary>
/// сущность тэгов, которые прикрепляются к группам
/// </summary>
public class Tag : BaseEntityDal<Guid>
{
    public required string Name { get; set; }
    public string? NormalizedName { get; set; }
    public required string Description { get; set; }

    public ICollection<Group> Groups { get; set; } = null!;
}