using System.Text.RegularExpressions;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Primitives;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;

namespace AspNetNetwork.Domain.Common.ValueObjects;

/// <summary>
/// Represents the emailAddress value object.
/// </summary>
public sealed class EmailAddress : ValueObject
{
    /// <summary>
    /// The emailAddress maximum length.
    /// </summary>
    public const int MaxLength = 256;

    private const string EmailRegexPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

    private static readonly Lazy<Regex> EmailFormatRegex =
        new Lazy<Regex>(() => new Regex(EmailRegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailAddress"/> class.
    /// </summary>
    /// <param name="value">The emailAddress value.</param>
    public EmailAddress(string value) => Value = value;

    /// <summary>
    /// Gets the emailAddress value.
    /// </summary>
    public string Value { get; }

    public static implicit operator string(EmailAddress emailAddress) => emailAddress.Value;

    /// <summary>
    /// Creates a new <see cref="EmailAddress"/> instance based on the specified value.
    /// </summary>
    /// <param name="email">The emailAddress value.</param>
    /// <returns>The result of the emailAddress creation process containing the emailAddress or an error.</returns>
    public static Result<EmailAddress> Create(string email) =>
        Result.Create(email, DomainErrors.Email.NullOrEmpty)
            .Ensure(e => !string.IsNullOrWhiteSpace(e), DomainErrors.Email.NullOrEmpty)
            .Ensure(e => e.Length <= MaxLength, DomainErrors.Email.LongerThanAllowed)
            .Ensure(e => EmailFormatRegex.Value.IsMatch(e), DomainErrors.Email.InvalidFormat)
            .Map(e => new EmailAddress(e));

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}