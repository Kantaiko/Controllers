using System;
using System.Collections.Generic;
using System.Threading;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Properties;
using Kantaiko.Properties.Immutable;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution;

public class ControllerExecutionContext<TContext> : IPropertyContainer where TContext : IContext
{
    public ControllerExecutionContext(TContext requestContext, IntrospectionInfo introspectionInfo)
    {
        RequestContext = requestContext;
        IntrospectionInfo = introspectionInfo;
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

    public IServiceProvider ServiceProvider => RequestContext.ServiceProvider;
    public CancellationToken CancellationToken => RequestContext.CancellationToken;
}
