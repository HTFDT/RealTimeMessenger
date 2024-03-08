using Domain.Interfaces;

namespace Infrastructure.Repositories;

internal class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}