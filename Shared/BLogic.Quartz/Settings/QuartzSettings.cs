namespace BLogic.Quartz.Settings;

public sealed class QuartzSettings
{
    public List<JobsSettings> Jobs { get; set; } = [];
}

public sealed class JobsSettings
{
    public string JobType { get; set; }
    public string CronExpression { get; set; }
    public string TimeZone { get; set; }
    public bool IsEnabled { get; set; }
}