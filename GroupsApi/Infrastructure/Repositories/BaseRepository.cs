using System.Linq.Expressions;
using Core.Base.Dal;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal abstract class BaseRepository<TEntity>(ApplicationDbContext context) : IRepository<TEntity>
    where TEntity : BaseEntityDal<Guid>
{
    protected readonly DbSet<TEntity> Entities = context.Set<TEntity>();

    public Task CreateAsync(TEntity entity)
    {
        Entities.Add(entity);
        return Task.CompletedTask;
    }
    
    public Task CreateBulkAsync(IEnumerable<TEntity> entities)
    {
        Entities.AddRange(entities);
        return Task.CompletedTask;
    }

    public Task<TEntity?> GetByIdAsync(Guid id)
    {
        return Entities.SingleOrDefaultAsync(e => e.Id == id);
    }

    public Task UpdateAsync(TEntity entity)
    {
        Entities.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        Entities.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return Entities.Where(predicate).ToListAsync();
    }

    public Task<List<TEntity>> GetAllAsync()
    {
        return Entities.ToListAsync();
    }

    public Task LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> property)
        where TProperty : class
    {
        if (!Entities.Local.Contains(entity))
            Entities.Attach(entity);
        return context.Entry(entity).Reference(property).LoadAsync();
    }

    public Task LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> property)
        where TProperty : class
    {
        if (!Entities.Local.Contains(entity))
            Entities.Attach(entity);
        return context.Entry(entity).Collection(property).LoadAsync();
    }
}