namespace Kantaiko.Controllers.Introspection.Context;

/// <summary>
/// The context passed to parameter transformation methods and various contracts like filters, property providers, etc.
/// </summary>
public readonly ref struct ParameterTransformationContext
{
    /// <summary>
    /// Creates a new instance of <see cref="ParameterTransformationContext"/>.
    /// </summary>
    /// <param name="parameter">The parameter that is being transformed.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public ParameterTransformationContext(EndpointParameterInfo parameter, IServiceProvider serviceProvider)
    {
        Parameter = parameter;
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// The parameter that is being transformed.
    /// </summary>
    public EndpointParameterInfo Parameter { get; }

    /// <summary>
    /// The service provider.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }
}
