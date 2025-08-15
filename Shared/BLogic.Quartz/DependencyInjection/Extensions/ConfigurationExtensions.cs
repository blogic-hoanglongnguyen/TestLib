using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLogic.Quartz.DependencyInjection.Extensions;

public static class ConfigurationExtensions
{
    public static T AddSingletonOptions<T>(this IServiceCollection services, IConfiguration configuration,
        string sectionName) where T : class
    {
        var section = configuration.GetSection(sectionName);
        var options = (T)Activator.CreateInstance(typeof(T));
        section.Bind(options);
        services.AddSingleton<T>(options);
        return options;
    }
}