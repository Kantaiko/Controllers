using System;
using System.Collections.Generic;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection;

public record ControllerInfo : IImmutablePropertyContainer
{
    private readonly IReadOnlyList<EndpointInfo> _endpoints;

    public ControllerInfo(Type type, IReadOnlyList<EndpointInfo> endpoints,
        IImmutablePropertyCollection? properties = null)
    {
        Type = type;
        _endpoints = AddParentReferences(endpoints);
        Properties = properties ?? ImmutablePropertyCollection.Empty;
    }

    public IReadOnlyList<EndpointInfo> Endpoints
    {
        get => _endpoints;
        init => _endpoints = AddParentReferences(value);
    }

    public Type Type { get; init; }
    public IImmutablePropertyCollection Properties { get; init; }

    public IntrospectionInfo? IntrospectionInfo { get; internal set; }

    private IReadOnlyList<EndpointInfo> AddParentReferences(IReadOnlyList<EndpointInfo> endpoints)
    {
        foreach (var endpoint in endpoints)
        {
            endpoint.Controller = this;
        }

        return endpoints;
    }
}
