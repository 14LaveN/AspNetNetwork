using System.ComponentModel.DataAnnotations;

namespace AspNetNetwork.Domain.Common.Entities;

/// <summary>
/// Represents metric entity class.
/// </summary>
public sealed class MetricEntity : BaseMongoEntity
{
    /// <summary>
    /// Initialize the <see cref="MetricEntity"/> class.
    /// </summary>
    /// <param name="name">The metric name.</param>
    /// <param name="description">The metric Description.</param>
    public MetricEntity(string name, string description)
    {
        Name = name;
        Description = description;
    }
    
    /// <summary>
    /// Gets or sets metric name.
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets metric description.
    /// </summary>
    [Required]
    public string Description { get; set; }
    
    /// <summary>
    /// Mutate the description and name to <see cref="MetricEntity"/>
    /// </summary>
    /// <param name="description">The metric Description.</param>
    /// <param name="name">The metric name.</param>
    /// <returns>Returns the <see cref="MetricEntity"/>.</returns>
    public static MetricEntity ToMetricEntity(string description, string name)
    {
        return new MetricEntity(name, description);
    }
}