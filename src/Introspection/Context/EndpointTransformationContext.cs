namespace Kantaiko.Controllers.Introspection.Context;

/// <summary>
/// The context passed to endpoint transformation methods and various contracts like filters, property providers, etc.
/// </summary>
public readonly ref struct EndpointTransformationContext
{
    /// <summary>
    /// Creates a new instance of <see cref="EndpointTransformationContext"/>.
    /// </summary>
    /// <param name="endpoint">The endpoint that is being transformed.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public EndpointTransformationContext(EndpointInfo endpoint, IServiceProvider serviceProvider)
    {
        Endpoint = endpoint;
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// The endpoint that is being transformed.
    /// </summary>
    public EndpointInfo Endpoint { get; }

    /// <summary>
    /// The service provider.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }
}
