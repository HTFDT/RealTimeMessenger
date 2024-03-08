using Domain.Interfaces;

namespace Infrastructure.Repositories;

internal class RepositoryManager(ApplicationDbContext context) : IRepositoryManager
{
    private readonly Lazy<IGroupsRepository> _lazyGroupsRepository = new(() => new GroupsRepository(context));
    private readonly Lazy<IMessagesRepository> _lazyMessagesRepository = new(() => new MessagesRepository(context));
    private readonly Lazy<IMembershipsRepository> _lazyMembershipsRepository = new(() => new MembershipsRepository(context));
    private readonly Lazy<IGroupRolesRepository> _lazyGroupRolesRepository = new(() => new GroupRolesRepository(context));
    private readonly Lazy<IGroupRightsRepository> _lazyGroupRightsRepository = new(() => new GroupRightsRepository(context));
    private readonly Lazy<ITagsRepository> _lazyTagsRepository = new(() => new TagsRepository(context));
    private readonly Lazy<IUnitOfWork> _lazyUnitOfWork = new(() => new UnitOfWork(context));
    
    public IGroupsRepository Groups => _lazyGroupsRepository.Value;
    public IMessagesRepository Messages => _lazyMessagesRepository.Value;
    public ITagsRepository Tags => _lazyTagsRepository.Value;
    public IMembershipsRepository Memberships => _lazyMembershipsRepository.Value;
    public IGroupRightsRepository GroupRights => _lazyGroupRightsRepository.Value;
    public IGroupRolesRepository GroupRoles => _lazyGroupRolesRepository.Value;
    public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
}