using System.ComponentModel.DataAnnotations;
using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Controllers.Introspection.Transformers;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.ParameterValidation;

/// <summary>
/// The transformer that adds validators and validation attributes to the parameter properties.
/// </summary>
public sealed class ParameterValidationTransformer : IntrospectionInfoTransformer
{
    protected override IImmutablePropertyCollection TransformParameterProperties(ParameterTransformationContext context)
    {
        var attributes = context.Parameter.AttributeProvider.GetCustomAttributes(true);

        var validators = attributes
            .OfType<IParameterValidator>()
            .ToArray();

        var validationAttributes = attributes
            .OfType<ValidationAttribute>()
            .ToArray();

        return context.Parameter.Properties.Set(new ParameterValidationProperties(validators, validationAttributes));
    }
}
