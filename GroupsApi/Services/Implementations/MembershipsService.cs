using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using IdentityConnectionLib.ConnectionServices.Dtos;
using IdentityConnectionLib.ConnectionServices.Interfaces;
using Services.Attributes;
using Services.Interfaces.Interfaces;
using Shared.Dto.Groups;
using Shared.Dto.Memberships;

namespace Services.Implementations;

internal class MembershipsService(IRepositoryManager repoManager, IIdentityConnectionService identityConnectionService) : IMembershipsService
{
    public async Task<MembershipResponse> JoinGroup(Guid requesterId, Guid groupId)
    {
        var group = await CheckGroupExists(groupId);
        if (group.IsPrivate)
            throw new JoinPrivateGroupIsForbiddenException(group.Id);
        return await AddMember(requesterId, group);
    }

    // отличается только наличием проверки прав добавляющего
    public async Task<MembershipResponse> AddMemberToPublicGroup(Guid requesterId, Guid groupId, AddMemberRequest request)
    {
        var group = await CheckGroupExists(groupId);
        return await AddMember(request.UserId, group);
    }
    
    [EnsureRequesterRights("AddMembers")]
    public async Task<MembershipResponse> AddMemberToPrivateGroup(Guid requesterId, Guid groupId, AddMemberRequest request)
    {
        var group = await CheckGroupExists(groupId);
        return await AddMember(request.UserId, group);
    }

    private async Task<MembershipResponse> AddMember(Guid userId, Group group)
    {
        var user = await GetUserInfoFromUserId(userId);
        var member = await repoManager.Memberships.GetByUserIdAndGroupId(userId, group.Id);
        // проверяем, существет ли дефолтная роль для пользователей группы
        var defaultMemberRole = await repoManager.GroupRoles.GetByIdAsync(group.DefaultMemberRoleId);
        if (defaultMemberRole is null)
            throw new GroupRoleNotFoundException(group.Id, group.DefaultMemberRoleId);
        // проверяем, состоит ли пользователь в группе
        if (member is not null)
        {
            // проверяем, не кикнули ли пользователя оттуда
            if (member.IsKicked)
                throw new MemberIsKickedForbiddenException(userId);
            // проверяем, не вышел ли пользователь из группы сам
            if (!member.IsLeft)
                throw new MemberIsAlreadyInGroupConflictException(userId);
            // если вышел сам, восстанавливаем ему его прошлую роль
            member.IsLeft = false;
            // проверяем, не удалили ли прошлую роль
            // если удалили, то присваиваем роль обычного пользователя
            var roleToSet = member.GroupRoleBeforeLeaveId ?? defaultMemberRole.Id;
            member.GroupRoleId = roleToSet;
            member.GroupRoleBeforeLeaveId = null;
            member.IsLeft = false;
            await repoManager.Memberships.UpdateAsync(member);
            await repoManager.UnitOfWork.SaveChangesAsync();
            return new MembershipResponse(member.Id,
                member.UserId,
                user.Username,
                member.GroupId,
                member.GroupRoleId,
                member.DateJoined,
                member.IsKicked,
                member.DateKicked);
        }
        
        // создаём нового члена группы
        var newMember = new Membership
        {
            UserId = userId,
            GroupId = group.Id,
            GroupRoleId = defaultMemberRole.Id,
            IsRoleUnique = defaultMemberRole.IsUnique,
            DateJoined = DateTime.UtcNow,
            LastMessageNumberWhenJoined = group.LastMessageNum,
            IsKicked = false,
            IsLeft = false,
            IsOwner = false
        };
        await repoManager.Memberships.CreateAsync(newMember);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new MembershipResponse(newMember.Id,
            newMember.UserId,
            user.Username,
            newMember.GroupId,
            newMember.GroupRoleId,
            newMember.DateJoined,
            newMember.IsKicked,
            newMember.DateKicked);
    }
    
    [EnsureRequesterRights("KickMembers")]
    public async Task<MembershipResponse> KickMember(Guid requesterId, Guid groupId, Guid kickedId)
    {
        var user = await GetUserInfoFromUserId(kickedId);
        var group = await CheckGroupExists(groupId);
        var member = await CheckMemberExists(kickedId, group.Id);
        if (member.IsOwner)
            throw new OwnerKickForbiddenException();
        var kickedRole = await repoManager.GroupRoles.GetDefaultKickedRole();
        if (kickedRole is null)
            throw new DefaultKickedRoleNotFoundException();
        member.IsKicked = true;
        member.GroupRoleId = kickedRole.Id;
        member.DateKicked = DateTime.UtcNow;
        await repoManager.Memberships.UpdateAsync(member);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new MembershipResponse(member.Id,
            member.UserId,
            user.Username,
            member.GroupId,
            member.GroupRoleId,
            member.DateJoined,
            member.IsKicked,
            member.DateKicked);
    }
    
    [EnsureRequesterRights("ViewMembers")]
    public async Task<IEnumerable<MembershipResponse>> GetAllMembers(Guid requesterId, Guid groupId)
    {
        var group = await CheckGroupExists(groupId);
        await repoManager.Groups.LoadCollection(group, gr => gr.Memberships);
        var ids = group.Memberships.Select(m => m.UserId);
        var userInfos = (await identityConnectionService.GetUserInfos(ids))
            .ToDictionary(x => x.Id, x => x.Username);
        
        return group.Memberships.Select(member => new MembershipResponse(member.Id,
            member.UserId,
            userInfos[member.UserId],
            member.GroupId,
            member.GroupRoleId,
            member.DateJoined,
            member.IsKicked,
            member.DateKicked));
    }
    
    [EnsureRequesterRights("ViewMembers")]
    public async Task<MembershipResponse> GetMember(Guid requesterId, Guid groupId, Guid memberId)
    {
        await CheckGroupExists(groupId);
        var member = await CheckMemberExists(memberId, groupId);
        var user = await GetUserInfoFromUserId(member.UserId);
        return new MembershipResponse(member.Id,
            member.UserId,
            user.Username,
            member.GroupId,
            member.GroupRoleId,
            member.DateJoined,
            member.IsKicked,
            member.DateKicked);
    }

    public async Task<MembershipResponse> LeaveGroup(Guid requesterId, Guid groupId)
    {
        var group = await CheckGroupExists(groupId);
        var member = await CheckUserIsMember(groupId, requesterId);
        var user = await GetUserInfoFromUserId(member.Id);
        if (member.IsOwner)
            throw new OwnerLeaveForbiddenException();
        var leftRole = await repoManager.GroupRoles.GetDefaultLeftRole();
        if (leftRole is null)
            throw new DefaultLeftRoleNotFoundException();
        member.GroupRoleBeforeLeaveId = member.GroupRoleId;
        member.GroupRoleId = leftRole.Id;
        member.IsLeft = true;
        await repoManager.Memberships.UpdateAsync(member);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new MembershipResponse(member.Id,
            member.UserId,
            user.Username,
            member.GroupId,
            member.GroupRoleId,
            member.DateJoined,
            member.IsKicked,
            member.DateKicked);
    }
    
    [EnsureRequesterRights("ManageRoles")]
    public async Task<MembershipResponse> AssignRoleToMember(Guid requesterId, Guid groupId, Guid memberId, AssignRoleToMemberRequest request)
    {
        var group = await CheckGroupExists(groupId);
        var member = await CheckMemberExists(memberId, group.Id);
        var user = await GetUserInfoFromUserId(member.Id);
        await repoManager.Memberships.LoadReference(member, m => m.GroupRole);
        if (!member.GroupRole.IsRevocable)
            throw new RoleIsNotRevocableForbiddenException(member.GroupRole.Id);
        var role = await repoManager.GroupRoles.GetByGroupIdAndRoleId(group.Id, request.GroupRoleId) ?? 
                   await repoManager.GroupRoles.GetByIdAsync(request.GroupRoleId);
        if (role is null)
            throw new GroupRoleNotFoundException(group.Id, request.GroupRoleId);

        if (!role.IsAssignableByUsers)
            throw new RoleIsNotAssignableByUsersForbiddenException(role.Id);

        member.GroupRoleId = role.Id;
        member.IsKicked = role.IsDefaultKickedRole;
        await repoManager.Memberships.UpdateAsync(member);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new MembershipResponse(member.Id,
            member.UserId,
            user.Username,
            member.GroupId,
            member.GroupRoleId,
            member.DateJoined,
            member.IsKicked,
            member.DateKicked);
    }
    
    private async Task<Group> CheckGroupExists(Guid groupId)
    {
        var group = await repoManager.Groups.GetByIdAsync(groupId);
        if (group is null)
            throw new GroupNotFoundException(groupId);
        return group;
    }
    
    private async Task<Membership> CheckMemberExists(Guid memberId, Guid groupId)
    {
        var member = await repoManager.Memberships.GetByMemberIdAndGroupId(groupId, memberId);
        if (member is null)
            throw new MemberNotFoundException(memberId, groupId);
        return member;
    }
    
    private async Task<Membership> CheckUserIsMember(Guid groupId, Guid userId)
    {
        var member = await repoManager.Memberships.GetByUserIdAndGroupId(userId, groupId);
        if (member is null)
            throw new NotAMemberNotFoundException(userId, groupId);
        return member;
    }
    
    private async Task<UserInfoResponse> GetUserInfoFromUserId(Guid userId)
    {
        var userInfo = await identityConnectionService.GetUserInfo(userId);
        return userInfo;
    }
}