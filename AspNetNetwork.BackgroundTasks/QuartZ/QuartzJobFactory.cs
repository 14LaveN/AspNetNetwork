using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace AspNetNetwork.BackgroundTasks.QuartZ;

public sealed class QuartzJobFactory(IServiceScopeFactory serviceScopeFactory)
    : IJobFactory
{
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        using var scope = serviceScopeFactory.CreateScope();
        
        var job = scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
        return job!;
    }

    public void ReturnJob(IJob job)
    {
        var disposable = job as IDisposable;
        disposable?.Dispose();
    }
}