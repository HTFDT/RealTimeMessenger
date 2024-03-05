namespace Domain.Interfaces;

public interface IRepositoryManager
{
    IGroupsRepository Groups { get; }
    IMessagesRepository Messages { get; }
    ITagsRepository Tags { get; }
    IMembershipsRepository Memberships { get; }
    IGroupRightsRepository GroupRights { get; }
    IGroupRolesRepository GroupRoles { get; }
    IUnitOfWork UnitOfWork { get; }
}