using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.EndpointMatching;

/// <summary>
/// The context passed to endpoint matchers.
/// </summary>
public readonly ref struct EndpointMatchingContext
{
    private readonly ControllerExecutionContext _executionContext;

    internal EndpointMatchingContext(ControllerExecutionContext executionContext, EndpointInfo endpoint)
    {
        _executionContext = executionContext;
        Endpoint = endpoint;
    }

    /// <summary>
    /// The current request context.
    /// </summary>
    public object RequestContext => _executionContext.RequestContext;

    /// <summary>
    /// The endpoint that is being matched.
    /// </summary>
    public EndpointInfo Endpoint { get; }

    /// <summary>
    /// The property collection of the execution context.
    /// </summary>
    public IPropertyCollection Properties => _executionContext.Properties;

    /// <summary>
    /// The service provider.
    /// </summary>
    public IServiceProvider ServiceProvider => _executionContext.ServiceProvider;
}

/// <summary>
/// The generic version of <see cref="EndpointMatchingContext"/>.
/// </summary>
/// <typeparam name="TContext">The type of the request context.</typeparam>
public readonly ref struct EndpointMatchingContext<TContext>
{
    internal EndpointMatchingContext(TContext requestContext, EndpointInfo endpoint,
        IServiceProvider serviceProvider)
    {
        RequestContext = requestContext;
        Endpoint = endpoint;
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// The current request context.
    /// </summary>
    public TContext RequestContext { get; }

    /// <summary>
    /// The endpoint that is being matched.
    /// </summary>
    public EndpointInfo Endpoint { get; }

    /// <summary>
    /// The service provider.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }
}
