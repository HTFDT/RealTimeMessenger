using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Services.Interfaces.Interfaces;
using Shared.Dto.Tags;

namespace Services.Implementations;

internal class TagsService(IRepositoryManager repoManager) : ITagsService
{
    public async Task<IEnumerable<TagResponse>> GetAllTags()
    {
        var tags = await repoManager.Tags.GetAllAsync();
        return tags.Select(t => new TagResponse(t.Id, t.Name, t.Description));
    }

    public async Task<TagResponse> GetTag(Guid tagId)
    {
        var tag = await CheckTagExists(tagId);
        return new TagResponse(tag.Id, tag.Name, tag.Description);
    }

    public async Task<TagResponse> CreateTag(CreateTagRequest request)
    {
        await CheckTagNameUnique(request.Name);
        var newTag = new Tag
        {
            Name = request.Name,
            Description = request.Description,
        };
        await repoManager.Tags.CreateAsync(newTag);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new TagResponse(newTag.Id, newTag.Name, newTag.Description);
    }

    public async Task<TagResponse> UpdateTag(Guid tagId, UpdateTagRequest request)
    {
        var tag = await CheckTagExists(tagId);
        if (tag.NormalizedName != request.Name.ToUpper()) 
            await CheckTagNameUnique(request.Name);
        tag.Name = request.Name;
        tag.Description = request.Description;
        await repoManager.Tags.UpdateAsync(tag);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new TagResponse(tag.Id, tag.Name, tag.Description);
    }

    public async Task DeleteTag(Guid tagId)
    {
        var tag = await CheckTagExists(tagId);
        await repoManager.Tags.DeleteAsync(tag);
        await repoManager.UnitOfWork.SaveChangesAsync();
    }

    private async Task<Tag> CheckTagExists(Guid tagId)
    {
        var tag = await repoManager.Tags.GetByIdAsync(tagId);
        if (tag is null)
            throw new TagNotFoundException(tagId);
        return tag;
    }
    
    private async Task CheckTagNameUnique(string tagName)
    {
        var tag = await repoManager.Tags.GetByTagNameAsync(tagName);
        if (tag is not null)
            throw new TagNameConflictException(tagName);
    }
}