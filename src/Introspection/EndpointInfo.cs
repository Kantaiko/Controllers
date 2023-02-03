using System.Collections.Immutable;
using System.Reflection;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Utils;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection;

/// <summary>
/// Contains static information about an endpoint.
/// </summary>
public sealed record EndpointInfo : IImmutablePropertyContainer
{
    private readonly IReadOnlyList<EndpointParameterInfo> _parameterTree;
    private ControllerInfo? _controller;

    internal EndpointInfo(MethodInfo methodInfo, IReadOnlyList<EndpointParameterInfo> parameterTree,
        IImmutablePropertyCollection? properties = null)
    {
        MethodInfo = methodInfo;

        _parameterTree = parameterTree;
        Parameters = AddParentReferences(Flatten(parameterTree).ToImmutableArray());

        Properties = properties ?? ImmutablePropertyCollection.Empty;
    }

    /// <summary>
    /// The list of first-level parameters of the endpoint.
    /// </summary>
    public IReadOnlyList<EndpointParameterInfo> ParameterTree
    {
        get => _parameterTree;
        init
        {
            _parameterTree = value;
            Parameters = AddParentReferences(Flatten(value).ToImmutableArray());
        }
    }

    /// <summary>
    /// The method info of the endpoint.
    /// </summary>
    public MethodInfo MethodInfo { get; init; }

    /// <summary>
    /// The flattened list of all parameters of the endpoint.
    /// <br/>
    /// If the endpoint does not contain composite parameters,
    /// this list will contain the same elements as <see cref="ParameterTree"/>.
    /// </summary>
    public IReadOnlyList<EndpointParameterInfo> Parameters { get; private init; }

    /// <summary>
    /// The collection of user-defined properties associated with the endpoint.
    /// </summary>
    public IImmutablePropertyCollection Properties { get; init; }

    /// <summary>
    /// The reference to the controller that contains this endpoint.
    /// </summary>
    /// <exception cref="ParentNotLinkedException">Thrown when the controller is not set.</exception>
    public ControllerInfo Controller
    {
        get => _controller ?? throw new ParentNotLinkedException(this);
        internal set => _controller = value;
    }

    private static IEnumerable<EndpointParameterInfo> Flatten(IEnumerable<EndpointParameterInfo> parameters)
    {
        return parameters.SelectMany(parameterInfo =>
        {
            if (parameterInfo.HasChildren)
            {
                return Flatten(parameterInfo.Children);
            }

            return EnumerableUtils.Single(parameterInfo);
        });
    }

    private IReadOnlyList<EndpointParameterInfo> AddParentReferences(IReadOnlyList<EndpointParameterInfo> parameters)
    {
        foreach (var parameter in parameters)
        {
            parameter.Endpoint = this;
        }

        return parameters;
    }
}
