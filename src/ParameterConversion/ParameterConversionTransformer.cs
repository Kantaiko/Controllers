using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Controllers.Introspection.Transformers;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.ParameterConversion;

/// <summary>
/// The transformer that adds <see cref="ParameterConversionProperties"/> to the endpoint properties.
/// </summary>
public sealed class ParameterConversionTransformer : IntrospectionInfoTransformer
{
    protected override IImmutablePropertyCollection TransformParameterProperties(ParameterTransformationContext context)
    {
        var converters = context.Parameter.AttributeProvider.GetCustomAttributes(true)
            .OfType<IParameterConverter>()
            .ToArray();

        return context.Parameter.Properties.Set(new ParameterConversionProperties(converters));
    }
}
