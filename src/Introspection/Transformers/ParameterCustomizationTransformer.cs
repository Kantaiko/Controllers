using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Controllers.Introspection.Contracts;

namespace Kantaiko.Controllers.Introspection.Transformers;

/// <summary>
/// The transformer that applies customizations to parameters.
/// </summary>
public sealed class ParameterCustomizationTransformer : IntrospectionInfoTransformer
{
    protected override EndpointParameterInfo TransformParameter(ParameterTransformationContext context)
    {
        var customizationProvider = context.Parameter.AttributeProvider.GetCustomAttributes(true)
            .OfType<IParameterCustomizationProvider>()
            .LastOrDefault();

        if (customizationProvider is null)
        {
            return context.Parameter with
            {
                Children = TransformParameters(context.Parameter.Children, context.ServiceProvider)
            };
        }

        var customization = customizationProvider.GetParameterCustomization(context);

        return context.Parameter with
        {
            Name = customization.Name ?? context.Parameter.Name,
            IsOptional = customization.IsOptional ?? context.Parameter.IsOptional,
            DefaultValue = customization.DefaultValue ?? context.Parameter.DefaultValue,
            Children = TransformParameters(context.Parameter.Children, context.ServiceProvider)
        };
    }
}
