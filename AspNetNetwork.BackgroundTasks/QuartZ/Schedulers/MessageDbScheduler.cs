using AspNetNetwork.BackgroundTasks.QuartZ.Jobs;

namespace AspNetNetwork.BackgroundTasks.QuartZ.Schedulers;

/// <summary>
/// Represents the message database scheduler class.
/// </summary>
public sealed class MessageDbScheduler
    : AbstractScheduler<MessageDbJob>;