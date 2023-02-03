using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.ParameterConversion;

/// <summary>
/// The context of the parameter conversion.
/// </summary>
public readonly struct ParameterConversionContext
{
    /// <summary>
    /// Creates a new instance of <see cref="ParameterConversionContext"/>.
    /// </summary>
    /// <param name="parameter">The parameter to convert.</param>
    /// <param name="executionContext">The execution context of the current request.</param>
    public ParameterConversionContext(EndpointParameterInfo parameter, ControllerExecutionContext executionContext)
    {
        Parameter = parameter;
        ExecutionContext = executionContext;
    }

    /// <summary>
    /// The parameter being converted.
    /// </summary>
    public EndpointParameterInfo Parameter { get; }

    /// <summary>
    /// The execution context of the current request.
    /// </summary>
    public ControllerExecutionContext ExecutionContext { get; }

    /// <summary>
    /// The request context.
    /// </summary>
    public object RequestContext => ExecutionContext.RequestContext;

    /// <summary>
    /// The match properties provided by the endpoint matcher.
    /// </summary>
    public IReadOnlyPropertyCollection MatchProperties => ExecutionContext.MatchProperties;

    /// <summary>
    /// The service provider.
    /// </summary>
    public IServiceProvider ServiceProvider => ExecutionContext.ServiceProvider;

    /// <summary>
    /// The cancellation token.
    /// </summary>
    public CancellationToken CancellationToken => ExecutionContext.CancellationToken;
}
