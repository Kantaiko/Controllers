namespace Kantaiko.Controllers.Execution;

/// <summary>
/// The implementation of the <see cref="IServiceProvider"/> that always returns null for any service.
/// </summary>
public sealed class EmptyServiceProvider : IServiceProvider
{
    private EmptyServiceProvider() { }

    /// <summary>
    /// The singleton instance of the <see cref="EmptyServiceProvider"/>.
    /// </summary>
    public static EmptyServiceProvider Instance { get; } = new();

    public object? GetService(Type serviceType)
    {
        return null;
    }
}
