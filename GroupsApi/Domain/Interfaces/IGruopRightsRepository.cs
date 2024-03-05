using Domain.Entities;

namespace Domain.Interfaces;

public interface IGroupRightsRepository : IRepository<GroupRight>
{
    Task<GroupRight?> GetByGroupRightNameAsync(string name);
}