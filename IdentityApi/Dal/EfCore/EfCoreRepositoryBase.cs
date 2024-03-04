using Core.Base.Dal;
using Dal.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.EfCore;

internal abstract class EfCoreRepositoryBase<TEntity>(ApplicationDbContext context) : IGuidRepository<TEntity> 
    where TEntity : BaseEntityDal<Guid>
{
    protected readonly DbSet<TEntity> Entities = context.Set<TEntity>();

    public virtual Guid Create(TEntity entity)
    {
        Entities.Add(entity);
        context.SaveChanges();
        return entity.Id;
    }

    public virtual TEntity? GetById(Guid id)
    {
        return Entities.AsNoTracking().SingleOrDefault(en => en.Id == id);
    }

    public virtual void Delete(TEntity entity)
    {
        Entities.Remove(entity);
        context.SaveChanges();
    }

    public virtual void Update(TEntity entity)
    {
        Entities.Update(entity);
        context.SaveChanges();
    }

    public virtual async Task<Guid> CreateAsync(TEntity entity)
    {
        await Entities.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public virtual Task<TEntity?> GetByIdAsync(Guid id)
    {
        return Entities.AsNoTracking().SingleOrDefaultAsync(en => en.Id == id);
    }

    public virtual Task DeleteAsync(TEntity entity)
    {
        Entities.Remove(entity);
        return context.SaveChangesAsync();
    }

    public virtual Task UpdateAsync(TEntity entity)
    {
        Entities.Remove(entity);
        return context.SaveChangesAsync();
    }

    public virtual List<TEntity> Filter(Func<TEntity, bool> predicate)
    {
        return Entities.Where(e => predicate(e)).ToList();
    }

    public virtual Task<List<TEntity>> FilterAsync(Func<TEntity, bool> predicate)
    {
        return Entities.Where(e => predicate(e)).ToListAsync();
    }

    public virtual List<TEntity> GetAll()
    {
        return Entities.ToList();
    }

    public virtual Task<List<TEntity>> GetAllAsync()
    {
        return Entities.ToListAsync();
    }

    public void DeleteById(Guid id)
    {
        var entity = GetById(id);
        if (entity is null)
            return;
        Delete(entity);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null)
            return;
        await DeleteAsync(entity);
    }
}