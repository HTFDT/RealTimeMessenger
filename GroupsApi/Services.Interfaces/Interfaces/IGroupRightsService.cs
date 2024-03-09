using Shared.Dto.GroupRights;

namespace Services.Interfaces.Interfaces;

public interface IGroupRightsService
{
    Task<GroupRightResponse> GetGroupRight(Guid groupRightId);
    Task<IEnumerable<GroupRightResponse>> GetAllGroupRights();
    Task<GroupRightResponse> UpdateGroupRight(Guid groupRightId, UpdateGroupRightRequest request);
    Task<GroupRightResponse> CreateGroupRight(Guid groupRightId, CreateGroupRightRequest request);
    Task DeleteGroupRight(Guid groupRightId);
}