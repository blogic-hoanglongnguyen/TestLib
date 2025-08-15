using BLogicCodeBase.Implementations;
using BLogicCodeBase.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BLogicCodeBase.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddBlogicCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IScopedCache, ScopedCache>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
    }
}