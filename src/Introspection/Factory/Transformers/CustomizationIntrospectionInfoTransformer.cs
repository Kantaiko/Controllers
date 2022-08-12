using System.Linq;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;

namespace Kantaiko.Controllers.Introspection.Factory.Transformers;

public class CustomizationIntrospectionInfoTransformer : IntrospectionInfoTransformer
{
    protected override EndpointParameterInfo TransformParameter(ParameterFactoryContext context)
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

        var (name, isOptional) = customizationProvider.GetParameterCustomization(context);

        return context.Parameter with
        {
            Name = name,
            IsOptional = isOptional,
            Children = TransformParameters(context.Parameter.Children, context.ServiceProvider)
        };
    }
}
