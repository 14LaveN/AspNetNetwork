namespace AspNetNetwork.Email.Contracts.Emails;

/// <summary>
/// Represents the done task email.
/// </summary>
public sealed class DoneTaskEmail
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoneTaskEmail"/> class.
    /// </summary>
    /// <param name="emailTo">The email receiver.</param>
    /// <param name="name">The name.</param>
    public DoneTaskEmail(string emailTo, string name)
    {
        EmailTo = emailTo;
        Name = name;
    }

    /// <summary>
    /// Gets the email receiver.
    /// </summary>
    public string EmailTo { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; }
}