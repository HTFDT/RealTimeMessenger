using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class TagsRepository(ApplicationDbContext context) : BaseRepository<Tag>(context), ITagsRepository
{
    public Task<Tag?> GetByTagNameAsync(string tagName)
    {
        return Entities.SingleOrDefaultAsync(e => e.NormalizedName == tagName.ToUpper());
    }
}