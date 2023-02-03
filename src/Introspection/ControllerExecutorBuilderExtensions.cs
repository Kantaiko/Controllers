using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Deconstruction;
using Kantaiko.Controllers.Introspection.Transformers;

namespace Kantaiko.Controllers.Introspection;

/// <summary>
/// The extension methods for <see cref="ControllerExecutorBuilder" />.
/// </summary>
public static class ControllerExecutorBuilderExtensions
{
    /// <summary>
    /// Adds default introspection transformers to the executor builder.
    /// </summary>
    /// <param name="builder">The executor builder to add transformers to.</param>
    public static void AddDefaultTransformers(this ControllerExecutorBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.DeconstructionValidator = new DefaultDeconstructionValidator();

        builder.Transformers.Add(new AttributeFilteringTransformer());
        builder.Transformers.Add(new ParameterCustomizationTransformer());
        builder.Transformers.Add(new PropertyProviderTransformer());
        builder.Transformers.Add(new VisibilityFilteringTransformer());
    }
}
