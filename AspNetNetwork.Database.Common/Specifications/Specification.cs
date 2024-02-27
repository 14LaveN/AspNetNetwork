using System.Linq.Expressions;
using AspNetNetwork.Domain.Common.Core.Primitives;

namespace AspNetNetwork.Database.Common.Specifications;

/// <summary>
/// Represents the abstract base class for specifications.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class Specification<TEntity>
    where TEntity : Entity
{
    /// <summary>
    /// Converts the specification to an expression predicate.
    /// </summary>
    /// <returns>The expression predicate.</returns>
    public abstract Expression<Func<TEntity, bool>> ToExpression();

    /// <summary>
    /// Checks if the specified entity satisfies this specification.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>True if the entity satisfies the specification, otherwise false.</returns>
    public bool IsSatisfiedBy(TEntity entity) => ToExpression().Compile()(entity);

    public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> specification) =>
        specification.ToExpression();
}