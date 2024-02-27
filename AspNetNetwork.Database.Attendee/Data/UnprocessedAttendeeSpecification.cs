using System.Linq.Expressions;
using AspNetNetwork.Database.Common.Specifications;

namespace AspNetNetwork.Database.Attendee.Data;

/// <summary>
/// Represents the specification for determining the unprocessed attendee.
/// </summary>
internal sealed class UnprocessedAttendeeSpecification : Specification<Domain.Identity.Entities.Attendee>
{
    /// <inheritdoc />
    public override Expression<Func<Domain.Identity.Entities.Attendee, bool>> ToExpression() => attendee => !attendee.Processed;
}