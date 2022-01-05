using System.Linq;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection.Factory.Transformers;

public class PropertyProviderIntrospectionInfoTransformer : IntrospectionInfoTransformer
{
    protected override IImmutablePropertyCollection TransformControllerProperties(ControllerFactoryContext context)
    {
        var propertyProviders = context.Controller.Type.GetCustomAttributes(true)
            .OfType<IControllerPropertyProvider>();

        var properties = context.Controller.Properties;

        foreach (var propertyProvider in propertyProviders)
        {
            properties = propertyProvider.UpdateControllerProperties(context);
        }

        return properties;
    }

    protected override IImmutablePropertyCollection TransformEndpointProperties(EndpointFactoryContext context)
    {
        var propertyProviders = context.Endpoint.MethodInfo.GetCustomAttributes(true)
            .OfType<IEndpointPropertyProvider>();

        var properties = context.Endpoint.Properties;

        foreach (var propertyProvider in propertyProviders)
        {
            properties = propertyProvider.UpdateEndpointProperties(context);
        }

        return properties;
    }

    protected override IImmutablePropertyCollection TransformParameterProperties(ParameterFactoryContext context)
    {
        var propertyProviders = context.Parameter.AttributeProvider.GetCustomAttributes(true)
            .OfType<IParameterPropertyProvider>();

        var properties = context.Parameter.Properties;

        foreach (var propertyProvider in propertyProviders)
        {
            properties = propertyProvider.UpdateParameterProperties(context);
        }

        return properties;
    }
}
