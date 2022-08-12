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
        ControllerExecutionContext<TContext> executionContext,
        EndpointParameterInfo parameter
    )
    {
        ExecutionContext = executionContext;
        Parameter = parameter;
    }

    public ControllerExecutionContext<TContext> ExecutionContext { get; }
    public TContext RequestContext => ExecutionContext.RequestContext;

    public EndpointParameterInfo Parameter { get; }

    private IPropertyCollection? _properties;
    public IPropertyCollection Properties => _properties ??= new PropertyCollection();

    public bool ValueExists { get; set; }
    public bool ValueResolved { get; set; }
    public object? ResolvedValue { get; set; }

    public IServiceProvider ServiceProvider => ExecutionContext.ServiceProvider;
    public CancellationToken CancellationToken => ExecutionContext.CancellationToken;
}
