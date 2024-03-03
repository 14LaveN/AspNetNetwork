using AspNetNetwork.Database.Message;
using Quartz;
using static System.Console;

namespace AspNetNetwork.BackgroundTasks.QuartZ.Jobs;

/// <summary>
/// Represents the user database job.
/// </summary>
public sealed class MessageDbJob : IJob
{
    private readonly MessageDbContext _appDbContext = new();

    /// <inheritdoc />
    public async Task Execute(IJobExecutionContext context)
    {
        await _appDbContext.SaveChangesAsync();
        WriteLine($"Messages.SaveChanges - {DateTime.UtcNow}");
    }
}