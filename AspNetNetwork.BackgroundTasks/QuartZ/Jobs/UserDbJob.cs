﻿using Quartz;
using AspNetNetwork.Database.Identity;
using static System.Console;

namespace AspNetNetwork.BackgroundTasks.QuartZ.Jobs;

/// <summary>
/// Represents the user database job.
/// </summary>
public sealed class UserDbJob : IJob
{
    private readonly UserDbContext _appDbContext = new();

    /// <inheritdoc />
    public async Task Execute(IJobExecutionContext context)
    {
        await _appDbContext.SaveChangesAsync();
        WriteLine($"User.SaveChanges - {DateTime.UtcNow}");
    }
}