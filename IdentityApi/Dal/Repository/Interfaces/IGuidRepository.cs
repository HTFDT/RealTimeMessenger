using Core.Base.Dal;

namespace Dal.Repository.Interfaces;

public interface IGuidRepository<TEntity> : IRepository<TEntity, Guid>
    where TEntity: BaseEntityDal<Guid>
{
    
}