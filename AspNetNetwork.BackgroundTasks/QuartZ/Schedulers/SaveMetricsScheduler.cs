using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using AspNetNetwork.BackgroundTasks.QuartZ.Jobs;

namespace AspNetNetwork.BackgroundTasks.QuartZ.Schedulers;

/// <summary>
/// Represents the save metrics scheduler class.
/// </summary>
public sealed class SaveMetricsScheduler
    : AbstractScheduler<SaveMetricsJob>
{
    /// <summary>
    /// Starts the job.
    /// </summary>
    public override async void Start(IServiceCollection serviceProvider)
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            //scheduler.JobFactory = serviceProvider.GetService<QuartzJobFactory>();
        await scheduler.Start();

        IJobDetail jobDetail = JobBuilder.Create<SaveMetricsJob>().Build();
        ITrigger trigger = TriggerBuilder
            .Create()
            .WithIdentity($"{nameof(SaveMetricsJob)}Trigger", "default")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(300)
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}