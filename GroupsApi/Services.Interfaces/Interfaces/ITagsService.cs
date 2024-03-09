using Shared.Dto.Tags;

namespace Services.Interfaces.Interfaces;

public interface ITagsService
{
    Task<IEnumerable<TagResponse>> GetAllTags();
    Task<TagResponse> GetTag(Guid tagId);
    Task<TagResponse> CreateTag(CreateTagRequest request);
    Task<TagResponse> UpdateTag(Guid tagId, UpdateTagRequest request);
    Task DeleteTag(Guid tagId);
}