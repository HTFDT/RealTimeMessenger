using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Services.Interfaces.Interfaces;
using Shared.Dto.GroupRights;

namespace Services.Implementations;

internal class GroupRightsService(IRepositoryManager repoManager) : IGroupRightsService
{
    public async Task<GroupRightResponse> GetGroupRight(Guid groupRightId)
    {
        var right = await CheckRightExists(groupRightId);
        return new GroupRightResponse(right.Id, right.Name, right.Description);
    }

    public async Task<IEnumerable<GroupRightResponse>> GetAllGroupRights()
    {
        var rights = await repoManager.GroupRights.GetAllAsync();
        return rights.Select(r => new GroupRightResponse(r.Id, r.Name, r.Description));
    }

    public async Task<GroupRightResponse> UpdateGroupRight(Guid groupRightId, UpdateGroupRightRequest request)
    {
        var right = await CheckRightExists(groupRightId);
        if (right.NormalizedName != request.Name.ToUpper())
            await CheckRightNameUnique(request.Name);
        right.Name = request.Name;
        right.Description = request.Description;
        await repoManager.GroupRights.UpdateAsync(right);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new GroupRightResponse(right.Id, right.Name, right.Description);
    }

    public async Task<GroupRightResponse> CreateGroupRight(Guid groupRightId, CreateGroupRightRequest request)
    {
        await CheckRightNameUnique(request.Name);
        var newRight = new GroupRight
        {
            Name = request.Name,
            Description = request.Description,
        };
        await repoManager.GroupRights.CreateAsync(newRight);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new GroupRightResponse(newRight.Id, newRight.Name, newRight.Description);
    }

    public async Task DeleteGroupRight(Guid groupRightId)
    {
        var right = await CheckRightExists(groupRightId);
        await repoManager.GroupRights.DeleteAsync(right);
        await repoManager.UnitOfWork.SaveChangesAsync();
    }

    private async Task<GroupRight> CheckRightExists(Guid groupRightId)
    {
        var right = await repoManager.GroupRights.GetByIdAsync(groupRightId);
        if (right is null)
            throw new GroupRightNotFoundException(groupRightId);
        return right;
    }
    
    private async Task CheckRightNameUnique(string rightName)
    {
        var right = await repoManager.GroupRights.GetByGroupRightNameAsync(rightName);
        if (right is not null)
            throw new GroupRightNameConflictException(rightName);
    }
}