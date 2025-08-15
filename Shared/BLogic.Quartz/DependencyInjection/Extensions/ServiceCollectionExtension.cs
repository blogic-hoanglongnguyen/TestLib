using BLogic.Quartz.Service;
using BLogic.Quartz.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace BLogic.Quartz.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddBLogicQuartzExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingletonOptions<QuartzSettings>(configuration, "Quartz");
        services.AddTransient<IJobSchedulerService, JobSchedulerService>();
        services.AddQuartz(); 
        services.AddQuartzHostedService();
    }
}