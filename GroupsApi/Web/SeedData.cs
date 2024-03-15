using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces.Interfaces;
using Shared.Dto.GroupRights;

namespace Web;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider services)
    {
        var manager = services.GetRequiredService<IRepositoryManager>();
        var rightsToEntities = await CreateSeededRights(manager);
        await CreateSeededDefaultGroupRoles(manager, rightsToEntities);
    }

    private static async Task<Dictionary<string, GroupRight>> CreateSeededRights(IRepositoryManager manager)
    {
        var rightsToCreate = new List<GroupRight>
        {
            new() { Name = "DeleteGroup", Description = "right to delete the group" },
            new() { Name = "EditGroup", Description = "right to edit group name, description and tags" },
            new() { Name = "AddMembers", Description = "right to add new members (works only in private groups)" },
            new() { Name = "KickMembers", Description = "right to kick members" },
            new() { Name = "ViewMembers", Description = "right to view the list of group members" },
            new() { Name = "ViewRoles", Description = "right to view the list of the group roles" },
            new() { Name = "ManageRoles", Description = "right to create, delete, edit, assign and revoke the group roles" },
            new() { Name = "SendMessages", Description = "right to send messages in the group chat" },
            new() { Name = "DeleteMessages", Description = "right to delete the other group members' messages" },
            new() { Name = "ViewMessages", Description = "right to view the list of the group chat messages" },
            new() { Name = "ViewHistory", Description = "right to view the chat history before the member joined the group (works only with the right to view messages)" },
            new() { Name = "ManagePins", Description = "right to pin and unpin the group chat messages" }
        };

        foreach (var right in rightsToCreate)
        {
            var existingRights = await manager.GroupRights.GetByGroupRightNameAsync(right.Name);
            if (existingRights is null)
                await manager.GroupRights.CreateAsync(right);
        }

        await manager.UnitOfWork.SaveChangesAsync();

        return rightsToCreate.ToDictionary(r => r.Name, r => r);
    }

    private static async Task CreateSeededDefaultGroupRoles(IRepositoryManager manager,
        Dictionary<string, GroupRight> groupRightNameToEntity)
    {
        var ownerRole = new GroupRole
        {
            IsRevocable = false,
            IsUnique = true,
            IsAssignableByUsers = false,
            IsDefault = true,
            IsDefaultOwnerRole = true,
            IsDefaultKickedRole = false,
            IsDefaultLeftRole = false,
            IsDefaultMemberRole = false,
            GroupId = null,
            Name = "Owner",
            Description = "default role for the owner of the group",
            GroupRights = groupRightNameToEntity.Values
        };
        var memberRole = new GroupRole
        {
            IsRevocable = true,
            IsUnique = false,
            IsAssignableByUsers = true,
            IsDefault = true,
            IsDefaultOwnerRole = false,
            IsDefaultKickedRole = false,
            IsDefaultLeftRole = false,
            IsDefaultMemberRole = true,
            GroupId = null,
            Name = "Member",
            Description = "default role for the member of the group",
            GroupRights = new List<GroupRight>
            {
                groupRightNameToEntity["ViewMessages"],
                groupRightNameToEntity["ViewHistory"],
                groupRightNameToEntity["SendMessages"],
                groupRightNameToEntity["AddMembers"],
                groupRightNameToEntity["ViewMembers"],
                groupRightNameToEntity["ViewRoles"]
            }
        };
        var kickedRole = new GroupRole
        {
            IsRevocable = true,
            IsUnique = false,
            IsAssignableByUsers = true,
            IsDefault = true,
            IsDefaultOwnerRole = false,
            IsDefaultKickedRole = true,
            IsDefaultLeftRole = false,
            IsDefaultMemberRole = false,
            GroupId = null,
            Name = "Kicked",
            Description = "default role for the kicked member of the group"
        };
        var leftRole = new GroupRole
        {
            IsRevocable = false,
            IsUnique = false,
            IsAssignableByUsers = false,
            IsDefault = true,
            IsDefaultOwnerRole = false,
            IsDefaultKickedRole = false,
            IsDefaultLeftRole = true,
            IsDefaultMemberRole = false,
            GroupId = null,
            Name = "Left",
            Description = "default role for the voluntarily left member of the group"
        };

        var existingOwnerRole = await manager.GroupRoles.GetDefaultOwnerRole();
        var existingMemberRole = await manager.GroupRoles.GetDefaultMemberRole();
        var existingKickedRole = await manager.GroupRoles.GetDefaultKickedRole();
        var existingLeftRole = await manager.GroupRoles.GetDefaultLeftRole();
        if (existingOwnerRole is null)
            await manager.GroupRoles.CreateAsync(ownerRole);
        if (existingMemberRole is null)
            await manager.GroupRoles.CreateAsync(memberRole);
        if (existingKickedRole is null)
            await manager.GroupRoles.CreateAsync(kickedRole);
        if (existingLeftRole is null)
            await manager.GroupRoles.CreateAsync(leftRole);
        
        await manager.UnitOfWork.SaveChangesAsync();
    }
}