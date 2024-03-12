using Domain.Interfaces;
using Services.Decorators;
using Services.Interfaces.Interfaces;

namespace Services.Implementations;

internal class ServicesManager(IRepositoryManager repoManager) : IServicesManager
{
    public IGroupsService Groups => _lazyGroups.Value;
    public IMessagesServiceWrapper Messages => _lazyMessages.Value;
    public IMembershipsServiceWrapper Memberships => _lazyMemberships.Value;
    public IGroupRolesService GroupRoles => _lazyGroupRoles.Value;
    public IGroupRightsService GroupRights => _lazyGroupRights.Value;
    public ITagsService Tags => _lazyTags.Value;

    private readonly Lazy<IGroupsService> _lazyGroups = new(() => new GroupsServiceEnsureRightsDecorator(new GroupsService(repoManager), repoManager));
    private readonly Lazy<IMessagesServiceWrapper> _lazyMessages = new(() => new MessagesServiceWrapper(new MessagesServiceEnsureRightsDecorator(new MessagesService(repoManager), repoManager), repoManager));
    private readonly Lazy<IMembershipsServiceWrapper> _lazyMemberships = new(() => new MembershipsServiceWrapper(new MembershipsServiceEnsureRightsDecorator(new MembershipsService(repoManager), repoManager), repoManager));
    private readonly Lazy<IGroupRolesService> _lazyGroupRoles = new(() => new GroupRolesServiceEnsureRightsDecorator(new GroupRolesService(repoManager), repoManager));
    private readonly Lazy<IGroupRightsService> _lazyGroupRights = new(() => new GroupRightsService(repoManager));
    private readonly Lazy<ITagsService> _lazyTags = new(() => new TagsService(repoManager));
}