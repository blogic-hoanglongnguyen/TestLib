using BLogic.Logging.Middlewares;
using Microsoft.Extensions.DependencyInjection;

namespace BLogic.Logging.DependencyInjection.Extensions;

public static class LoggingServiceExtensions
{
    public static void AddBLogicLogging(this IServiceCollection services)
    {
        services.AddScoped<SerilogMiddleware>();
    }
}