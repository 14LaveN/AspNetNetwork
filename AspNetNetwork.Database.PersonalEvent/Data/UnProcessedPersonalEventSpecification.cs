using System.Linq.Expressions;
using AspNetNetwork.Database.Common.Specifications;

namespace AspNetNetwork.Database.PersonalEvent.Data;

/// <summary>
/// Represents the specification for determining the unprocessed personal event.
/// </summary>
public sealed class UnProcessedPersonalEventSpecification : Specification<Domain.Identity.Entities.PersonalEvent>
{
    /// <inheritdoc />
    public override Expression<Func<Domain.Identity.Entities.PersonalEvent, bool>> ToExpression() => personalEvent => !personalEvent.Processed;
}