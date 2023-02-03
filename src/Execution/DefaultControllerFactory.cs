namespace Kantaiko.Controllers.Execution;

/// <summary>
/// The default implementation of <see cref="IControllerFactory"/> that
/// uses <see cref="Activator"/> to create instances and completely ignores
/// specified <see cref="IServiceProvider"/>.
/// </summary>
public sealed class DefaultControllerFactory : IControllerFactory
{
    private DefaultControllerFactory() { }

    /// <summary>
    /// The singleton instance of <see cref="DefaultControllerFactory"/>.
    /// </summary>
    public static DefaultControllerFactory Instance { get; } = new();

    public object CreateHandler(Type handlerType, IServiceProvider serviceProvider)
    {
        return Activator.CreateInstance(handlerType)!;
    }
}
