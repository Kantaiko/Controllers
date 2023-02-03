using Kantaiko.Controllers.Execution;

namespace Kantaiko.Controllers.EndpointMatching;

/// <summary>
/// The extension methods for <see cref="ControllerExecutorBuilder"/>.
/// </summary>
public static class ControllerExecutorBuilderExtensions
{
    /// <summary>
    /// Adds the endpoint matching transformers and handlers to the executor builder.
    /// </summary>
    /// <param name="builder">The executor builder.</param>
    public static void AddEndpointMatching(this ControllerExecutorBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Transformers.Add(new EndpointMatchingTransformer());
        builder.Handlers.Add(new EndpointMatchingHandler());
    }
}
