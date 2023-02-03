namespace Kantaiko.Controllers.Execution;

/// <summary>
/// The service that is responsible for creating controller instances by their types
/// using the specified <see cref="IServiceProvider"/>.
/// <br/>
/// Generally, this service should be implemented for each DI library using it's own
/// API to create instances.
/// </summary>
public interface IControllerFactory
{
    /// <summary>
    /// Creates a controller instance of the specified type.
    /// </summary>
    /// <param name="handlerType">The type of the controller to create.</param>
    /// <param name="serviceProvider">The service provider to use for creating the controller.</param>
    /// <returns>The created controller instance.</returns>
    object CreateHandler(Type handlerType, IServiceProvider serviceProvider);
}
