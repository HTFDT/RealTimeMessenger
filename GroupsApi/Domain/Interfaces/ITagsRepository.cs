using Domain.Entities;

namespace Domain.Interfaces;

public interface ITagsRepository : IRepository<Tag>
{
    Task<Tag?> GetByTagNameAsync(string tagName);
}