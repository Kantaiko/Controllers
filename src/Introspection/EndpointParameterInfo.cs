using System.Collections.Immutable;
using System.Reflection;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection;

/// <summary>
/// Contains static information about an endpoint.
/// </summary>
public sealed record EndpointParameterInfo : IImmutablePropertyContainer
{
    private readonly IReadOnlyList<EndpointParameterInfo> _children;
    private EndpointInfo? _endpoint;

    internal EndpointParameterInfo(ICustomAttributeProvider attributeProvider,
        string name, bool isOptional, Type parameterType, Type rawParameterType,
        object? defaultValue,
        IReadOnlyList<EndpointParameterInfo>? children = null,
        IImmutablePropertyCollection? properties = null)
    {
        AttributeProvider = attributeProvider;
        Name = name;
        IsOptional = isOptional;
        ParameterType = parameterType;
        RawParameterType = rawParameterType;
        DefaultValue = defaultValue;

        _children = children is not null ? AddParentReferences(children) : ImmutableArray<EndpointParameterInfo>.Empty;
        Properties = properties ?? ImmutablePropertyCollection.Empty;
    }

    /// <summary>
    /// The list of child parameters of the parameter.
    /// <br/>
    /// If the parameter is not a composite parameter, this list will be empty.
    /// </summary>
    public IReadOnlyList<EndpointParameterInfo> Children
    {
        get => _children;
        init => _children = AddParentReferences(value);
    }

    /// <summary>
    /// Whether the parameter is composite or not.
    /// <br/>
    /// Just a shortcut for <c>Children.Count > 0</c>.
    /// </summary>
    public bool HasChildren => Children.Count is not 0;

    /// <summary>
    /// The attribute provider of the parameter regardless composite or not.
    /// </summary>
    public ICustomAttributeProvider AttributeProvider { get; init; }

    /// <summary>
    /// The name of the parameter. Must be unique within the endpoint.
    /// <br/>
    /// Should be lower-camel-case by convention.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Whether the parameter is optional or not.
    /// </summary>
    public bool IsOptional { get; init; }

    /// <summary>
    /// The type of the parameter.
    /// </summary>
    public Type ParameterType { get; init; }

    /// <summary>
    /// The raw type of the parameter.
    /// For nullable value types, this will be the nullable type itself
    /// while <see cref="ParameterType"/> will be the underlying type.
    /// </summary>
    public Type RawParameterType { get; init; }

    /// <summary>
    /// The default value of the parameter.
    /// </summary>
    public object? DefaultValue { get; init; }

    /// <summary>
    /// The collection of user-defined properties associated with the parameter.
    /// </summary>
    public IImmutablePropertyCollection Properties { get; init; }

    /// <summary>
    /// The reference to the parent parameter of the parameter.
    /// <br/>
    /// Will be null if the parameter is a first-level parameter of the endpoint.
    /// </summary>
    public EndpointParameterInfo? Parent { get; internal set; }

    /// <summary>
    /// The reference to the endpoint that contains this parameter.
    /// </summary>
    /// <exception cref="ParentNotLinkedException">Thrown when the endpoint is not set.</exception>
    public EndpointInfo Endpoint
    {
        get => _endpoint ?? throw new ParentNotLinkedException(this);
        internal set => _endpoint = value;
    }

    private IReadOnlyList<EndpointParameterInfo> AddParentReferences(IReadOnlyList<EndpointParameterInfo> parameters)
    {
        foreach (var parameter in parameters)
        {
            parameter.Parent = this;
        }

        return parameters;
    }
}
