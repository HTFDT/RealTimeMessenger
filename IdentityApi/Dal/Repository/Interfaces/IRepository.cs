using Core.Base.Dal;

namespace Dal.Repository.Interfaces;

public interface IRepository<TEntity, TKey>
    where TEntity: BaseEntityDal<TKey>
{
    TKey Create(TEntity entity);
    TEntity? GetById(TKey id);
    void Delete(TEntity entity);
    void Update(TEntity entity);
    Task<TKey> CreateAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(TKey id);
    Task DeleteAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    List<TEntity> Filter(Func<TEntity, bool> predicate);
    Task<List<TEntity>> FilterAsync(Func<TEntity, bool> predicate);
    List<TEntity> GetAll();
    Task<List<TEntity>> GetAllAsync();
    void DeleteById(Guid id);
    Task DeleteByIdAsync(Guid id);
}