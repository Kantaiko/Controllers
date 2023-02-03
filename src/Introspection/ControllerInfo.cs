using Kantaiko.Controllers.Exceptions;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection;

/// <summary>
/// Contains static information about a controller.
/// </summary>
public sealed record ControllerInfo : IImmutablePropertyContainer
{
    private readonly IReadOnlyList<EndpointInfo> _endpoints;
    private IntrospectionInfo? _introspectionInfo;

    internal ControllerInfo(Type type, IReadOnlyList<EndpointInfo> endpoints,
        IImmutablePropertyCollection? properties = null)
    {
        Type = type;
        _endpoints = AddParentReferences(endpoints);
        Properties = properties ?? ImmutablePropertyCollection.Empty;
    }

    /// <summary>
    /// The list of endpoints defined in the controller.
    /// </summary>
    public IReadOnlyList<EndpointInfo> Endpoints
    {
        get => _endpoints;
        init => _endpoints = AddParentReferences(value);
    }

    /// <summary>
    /// The type of the controller class.
    /// </summary>
    public Type Type { get; init; }

    /// <summary>
    /// The collection of user-defined properties associated with the controller.
    /// </summary>
    public IImmutablePropertyCollection Properties { get; init; }

    /// <summary>
    /// The reference to the introspection info that contains this controller.
    /// </summary>
    /// <exception cref="ParentNotLinkedException">Thrown when the introspection info is not set.</exception>
    public IntrospectionInfo IntrospectionInfo
    {
        get => _introspectionInfo ?? throw new ParentNotLinkedException(this);
        internal set => _introspectionInfo = value;
    }

    private IReadOnlyList<EndpointInfo> AddParentReferences(IReadOnlyList<EndpointInfo> endpoints)
    {
        foreach (var endpoint in endpoints)
        {
            endpoint.Controller = this;
        }

        return endpoints;
    }
}
