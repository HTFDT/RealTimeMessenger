using Core.Base.Dal;

namespace Domain.Interfaces;

public interface IRepository<TEntity> : ITypedRepository<Guid, TEntity> 
    where TEntity : BaseEntityDal<Guid>
{
    
}