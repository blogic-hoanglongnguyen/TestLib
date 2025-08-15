# BLogic.Quartz

`BLogic.Quartz` is a module that integrates **Quartz.NET** into a .NET project following BLogic's standard structure.  
It provides an easy way to register and manage scheduled jobs.

---

## 1. Add refer to ``BLogic.Quartz.csporj``

### Add packages
Make sure you have added the Quartz packages to your project:
```shell
dotnet add package Quartz
dotnet add package Quartz.Extensions.Hosting
```

## 2. Add configuration ``appsettings.json``

```json
 "Quartz": {
    "Jobs": [
        {
            "JobType": "ClearUnusedFileJob", //Name
            "CronExpression": "0 0 0 * * ?", // Schedule config
            "TimeZone": "America/Los_Angeles", // Set timezone
            "IsEnabled": true // Set IsEnabled = true if the job is enabled
        }
    ]
},
```

## 3. Register dependency injection

```csharp
public static void AddServiceCollection(this IServiceCollection services, IConfiguration config)
{
    ...
    services.AddBLogicQuartzExtension(config);
    ...
}
```
## 4. Add new job
```csharp
public class SampleJob() : IJob
{
    //Implement bussines here
}
```


## 5. Add Job to pipe line in ``Program.cs``
```csharp
...
    
app.MapControllers();

using var scope = app.Services.CreateScope();
var scheduler = scope.ServiceProvider.GetRequiredService<IJobSchedulerService>();
await scheduler.ScheduleJobsAsync([assembly1, assembly2]); // Load all assembly containing jobs

app.Run();
```