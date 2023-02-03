using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.EndpointMatching;

/// <summary>
/// The context passed to endpoint matchers.
/// </summary>
public readonly ref struct EndpointMatchingContext
{
    internal EndpointMatchingContext(object requestContext, EndpointInfo endpoint, IServiceProvider serviceProvider)
    {
        RequestContext = requestContext;
        Endpoint = endpoint;
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// The current request context.
    /// </summary>
    public object RequestContext { get; }

    /// <summary>
    /// The endpoint that is being matched.
    /// </summary>
    public EndpointInfo Endpoint { get; }

    /// <summary>
    /// The service provider.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }
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
