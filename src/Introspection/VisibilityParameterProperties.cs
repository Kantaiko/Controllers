using Kantaiko.Properties;

namespace Kantaiko.Controllers.Introspection;

/// <summary>
/// The conventional way to mark parameters as visible or invisible.
/// Makes no sense to the internal logic of the framework, but can be used by other applications.
/// </summary>
public record VisibilityParameterProperties : PropertyRecord<VisibilityParameterProperties>
{
    /// <summary>
    /// Whether the parameter is hidden or not.
    /// <br/>
    /// Hidden parameters are still available in the introspection info,
    /// but should not be visible to the user.
    /// <br/>
    /// For example, a parameter can be hidden if it is used to inject a service.
    /// </summary>
    public bool IsHidden { get; init; }
}
