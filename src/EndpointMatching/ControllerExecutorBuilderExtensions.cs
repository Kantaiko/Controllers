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
    /// <param name="immediatelyReturnError">
    /// Whether to immediately return an error returned by the endpoint matcher.
    /// Otherwise, the error will be returned only if no other endpoint matches the request.
    /// If there are multiple endpoints returning errors, the last error will be returned.
    /// <br/>
    /// By default, the error is returned immediately.
    /// </param>
    public static void AddEndpointMatching(this ControllerExecutorBuilder builder, bool immediatelyReturnError = true)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Transformers.Add(new EndpointMatchingTransformer());
        builder.Handlers.Add(new EndpointMatchingHandler(immediatelyReturnError));
    }
}
