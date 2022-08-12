using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection;

public record EndpointParameterInfo : IImmutablePropertyContainer
{
    private readonly IReadOnlyList<EndpointParameterInfo> _children;

    public EndpointParameterInfo(ICustomAttributeProvider attributeProvider,
        string name, bool isOptional, Type parameterType,
        IReadOnlyList<EndpointParameterInfo>? children = null,
        IImmutablePropertyCollection? properties = null)
    {
        AttributeProvider = attributeProvider;
        Name = name;
        IsOptional = isOptional;
        ParameterType = parameterType;
        _children = children is not null ? AddParentReferences(children) : ImmutableArray<EndpointParameterInfo>.Empty;
        Properties = properties ?? ImmutablePropertyCollection.Empty;
    }

    public IReadOnlyList<EndpointParameterInfo> Children
    {
        get => _children;
        init => _children = AddParentReferences(value);
    }

    public ICustomAttributeProvider AttributeProvider { get; init; }
    public string Name { get; init; }
    public bool IsOptional { get; init; }
    public Type ParameterType { get; init; }
    public IImmutablePropertyCollection Properties { get; init; }

    public EndpointParameterInfo? Parent { get; internal set; }
    public EndpointInfo? Endpoint { get; internal set; }

    private IReadOnlyList<EndpointParameterInfo> AddParentReferences(IReadOnlyList<EndpointParameterInfo> parameters)
    {
        foreach (var parameter in parameters)
        {
            parameter.Parent = this;
        }

        return parameters;
    }

    public bool HasChildren => Children.Count is not 0;
}
