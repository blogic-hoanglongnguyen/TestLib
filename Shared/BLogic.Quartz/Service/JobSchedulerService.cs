using System.Reflection;
using BLogic.Helper.Reflections;
using BLogic.Quartz.Settings;
using Quartz;

namespace BLogic.Quartz.Service;

public interface IJobSchedulerService
{
    Task ScheduleJobsAsync(Assembly[] assemblies);
}

public sealed class JobSchedulerService(ISchedulerFactory schedulerFactory
    , QuartzSettings settings) : IJobSchedulerService
{
    private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;
    private readonly QuartzSettings _appSettings = settings;
    private Assembly[] _assemblies { get; set; }

    private Dictionary<string, Type> _jobTypes
    {
        get
        {
            var jobTypes = ReflectionCache.GetInheritedFromInterface(_assemblies, typeof(IJob))
                .Where(t =>  t.IsClass && !t.IsAbstract)
                .ToDictionary(t => t.Name, t => t);

            return jobTypes;
        }
    }

    public async Task ScheduleJobsAsync(Assembly[] assemblies)
    {
        _assemblies = assemblies;
        var scheduler = await _schedulerFactory.GetScheduler();

        foreach (var jobConfig in _appSettings.Jobs)
        {
            if (!jobConfig.IsEnabled || !_jobTypes.TryGetValue(jobConfig.JobType, out var jobType))
                continue;

            var jobKey = new JobKey(jobConfig.JobType);

            var job = JobBuilder.Create(jobType)
                .WithIdentity(jobKey)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{jobConfig.JobType}-trigger")
                .ForJob(jobKey)
                .StartNow()
                .WithCronSchedule(jobConfig.CronExpression, x =>
                    x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById(jobConfig.TimeZone)))
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}