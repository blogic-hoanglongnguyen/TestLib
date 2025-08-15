using BLogic.Cache.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLogic.Cache.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddBLogicCacheServices(this IServiceCollection services) 
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, InMemoryCacheService>();
    }
}