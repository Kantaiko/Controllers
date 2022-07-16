using System;
using System.Collections.Generic;
using System.Threading;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Properties;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Execution;

public class ControllerContext<TContext> : IPropertyContainer
{
    public ControllerContext(
        TContext requestContext,
        IntrospectionInfo introspectionInfo,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        RequestContext = requestContext;
        IntrospectionInfo = introspectionInfo;

        ServiceProvider = serviceProvider;
        CancellationToken = cancellationToken;
    }

    public TContext RequestContext { get; }
    public IntrospectionInfo IntrospectionInfo { get; }

    private IPropertyCollection? _properties;
    public IPropertyCollection Properties => _properties ??= new PropertyCollection();

    public EndpointInfo? Endpoint { get; set; }
    public IImmutablePropertyCollection? ParameterConversionProperties { get; set; }

    public object? ControllerInstance { get; set; }

    public Dictionary<EndpointParameterInfo, object?>? ResolvedParameters { get; set; }
    public object?[]? ConstructedParameters { get; set; }

    public object? RawResult { get; set; }
    public object? Result { get; set; }

    public IServiceProvider ServiceProvider { get; }
    public CancellationToken CancellationToken { get; }
}
