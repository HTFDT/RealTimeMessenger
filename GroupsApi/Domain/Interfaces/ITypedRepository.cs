using System.Linq.Expressions;
using Core.Base.Dal;

namespace Domain.Interfaces;

public interface ITypedRepository<TKey, TEntity> where TEntity: BaseEntityDal<TKey>
{
    Task CreateAsync(TEntity entity);
    Task CreateBulkAsync(IEnumerable<TEntity> entities);
    Task<TEntity?> GetByIdAsync(TKey id);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetAllAsync();
    Task LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> property)
        where TProperty : class;
    Task LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> property)
        where TProperty : class;
}