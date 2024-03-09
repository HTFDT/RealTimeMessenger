namespace Services.Interfaces.Interfaces;

public interface IServicesManager
{
    public IGroupsService Groups { get; }
    public IMessagesServiceWrapper Messages { get; }
    public IMembershipsServiceWrapper Memberships { get; }
    public IGroupRolesService GroupRoles { get; }
    public IGroupRightsService GroupRights { get; }
    public ITagsService Tags { get; }
}