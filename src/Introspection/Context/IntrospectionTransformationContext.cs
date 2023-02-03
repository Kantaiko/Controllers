namespace Kantaiko.Controllers.Introspection.Context;

/// <summary>
/// The context passed to introspection info transformation methods and various contracts like filters,
/// property providers, etc.
/// </summary>
public readonly ref struct IntrospectionTransformationContext
{
    /// <summary>
    /// Creates a new instance of <see cref="IntrospectionTransformationContext"/>.
    /// </summary>
    /// <param name="introspectionInfo">The introspection info that is being transformed.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public IntrospectionTransformationContext(IntrospectionInfo introspectionInfo, IServiceProvider serviceProvider)
    {
        IntrospectionInfo = introspectionInfo;
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// The introspection info that is being transformed.
    /// </summary>
    public IntrospectionInfo IntrospectionInfo { get; }

    /// <summary>
    /// The service provider.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }
}
