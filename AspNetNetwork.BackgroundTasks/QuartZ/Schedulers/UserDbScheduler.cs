using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using AspNetNetwork.BackgroundTasks.QuartZ.Jobs;

namespace AspNetNetwork.BackgroundTasks.QuartZ.Schedulers;

/// <summary>
/// Represents the user database scheduler class.
/// </summary>
public sealed class UserDbScheduler
    : AbstractScheduler<UserDbJob>;