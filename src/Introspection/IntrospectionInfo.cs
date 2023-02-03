using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection;

/// <summary>
/// The root class describing the introspection info.
/// Contains a list of controllers.
/// </summary>
public sealed record IntrospectionInfo : IImmutablePropertyContainer
{
    private readonly IReadOnlyList<ControllerInfo> _controllers;

    internal IntrospectionInfo(IReadOnlyList<ControllerInfo> controllers,
        IImmutablePropertyCollection? properties = null)
    {
        _controllers = AddParentReferences(controllers);
        Properties = properties ?? ImmutablePropertyCollection.Empty;
    }

    /// <summary>
    /// The list of controllers defined in the application.
    /// </summary>
    public IReadOnlyList<ControllerInfo> Controllers
    {
        get => _controllers;
        init => _controllers = AddParentReferences(value);
    }

    /// <summary>
    /// The collection of user-defined properties associated with the introspection info.
    /// </summary>
    public IImmutablePropertyCollection Properties { get; init; }

    private IReadOnlyList<ControllerInfo> AddParentReferences(IReadOnlyList<ControllerInfo> controllers)
    {
        foreach (var controller in controllers)
        {
            controller.IntrospectionInfo = this;
        }

        return controllers;
    }
}
