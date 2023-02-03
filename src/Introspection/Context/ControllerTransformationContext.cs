namespace Kantaiko.Controllers.Introspection.Context;

/// <summary>
/// The context passed to controller transformation methods and various contracts like filters, property providers, etc.
/// </summary>
public readonly ref struct ControllerTransformationContext
{
    /// <summary>
    /// Creates a new instance of <see cref="ControllerTransformationContext"/>.
    /// </summary>
    /// <param name="controller">The controller that is being transformed.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public ControllerTransformationContext(ControllerInfo controller, IServiceProvider serviceProvider)
    {
        Controller = controller;
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// The controller that is being transformed.
    /// </summary>
    public ControllerInfo Controller { get; }

    /// <summary>
    /// The service provider.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }
}
