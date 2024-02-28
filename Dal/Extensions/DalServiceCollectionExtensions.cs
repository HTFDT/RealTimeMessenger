using Dal.EfCore;
using Dal.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dal.Extensions;

public static class DalServiceCollectionExtensions
{
    public static IServiceCollection AddDalServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );
        services.AddTransient<IUserRepository, EfCoreUserRepository>();
        services.AddTransient<IRoleRepository, EfCoreRoleRepository>();
        return services;
    }
}