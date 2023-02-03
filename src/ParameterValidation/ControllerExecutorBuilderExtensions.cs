using Kantaiko.Controllers.Execution;

namespace Kantaiko.Controllers.ParameterValidation;

/// <summary>
/// The extensions for <see cref="ControllerExecutorBuilder"/>.
/// </summary>
public static class ControllerExecutorBuilderExtensions
{
    /// <summary>
    /// Adds the parameter validation transformers and handlers to the executor builder.
    /// </summary>
    /// <param name="builder">The executor builder.</param>
    public static void AddParameterValidation(this ControllerExecutorBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Transformers.Add(new ParameterValidationTransformer());
        builder.Handlers.Add(new ParameterValidationHandler());
    }
}
