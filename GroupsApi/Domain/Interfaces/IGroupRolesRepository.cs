using Domain.Entities;

namespace Domain.Interfaces;

public interface IGroupRolesRepository : IRepository<GroupRole>
{
    Task<List<GroupRole>> GetAllRolesInGroup(Guid groupId);
    Task<GroupRole?> GetByGroupIdAndRoleId(Guid groupId, Guid roleId);
    Task<GroupRole?> GetDefaultMemberRole();
    Task<GroupRole?> GetDefaultKickedRole();
    Task<GroupRole?> GetDefaultOwnerRole();
    Task<GroupRole?> GetDefaultLeftRole();
}