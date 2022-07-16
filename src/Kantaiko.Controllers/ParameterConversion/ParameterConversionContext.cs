using System;
using System.Threading;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Result;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.ParameterConversion;

public class ParameterConversionContext<TContext> : IPropertyContainer
{
    public ParameterConversionContext(
        ControllerContext<TContext> context,
        EndpointParameterInfo parameter
    )
    {
        Context = context;
        Parameter = parameter;
    }

    public ControllerContext<TContext> Context { get; }
    public TContext RequestContext => Context.RequestContext;
    public EndpointParameterInfo Parameter { get; }

    private IPropertyCollection? _properties;
    public IPropertyCollection Properties => _properties ??= new PropertyCollection();

    public bool ValueExists { get; set; }
    public bool ValueResolved { get; set; }
    public object? ResolvedValue { get; set; }

    public ControllerResult? ExecutionResult { get; set; }

    public IServiceProvider ServiceProvider => Context.ServiceProvider;
    public CancellationToken CancellationToken => Context.CancellationToken;
}
