using System.Linq;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;

namespace Kantaiko.Controllers.Introspection.Factory.Transformers;

public class PropertyProviderIntrospectionInfoTransformer : IntrospectionInfoTransformer
{
    protected override ControllerInfo TransformController(ControllerFactoryContext context)
    {
        var propertyProviders = context.Controller.Type.GetCustomAttributes(true)
            .OfType<IControllerPropertyProvider>();

        foreach (var propertyProvider in propertyProviders)
        {
            var properties = propertyProvider.UpdateControllerProperties(context);

            context = new ControllerFactoryContext(
                context.Controller with { Properties = properties },
                context.ServiceProvider
            );
        }

        return context.Controller with
        {
            Endpoints = TransformEndpoints(context)
        };
    }

    protected override EndpointInfo TransformEndpoint(EndpointFactoryContext context)
    {
        var propertyProviders = context.Endpoint.MethodInfo.GetCustomAttributes(true)
            .OfType<IEndpointPropertyProvider>();

        foreach (var propertyProvider in propertyProviders)
        {
            var properties = propertyProvider.UpdateEndpointProperties(context);

            context = new EndpointFactoryContext(
                context.Endpoint with { Properties = properties },
                context.ServiceProvider
            );
        }

        return context.Endpoint with
        {
            ParameterTree = TransformParameterTree(context)
        };
    }

    protected override EndpointParameterInfo TransformParameter(ParameterFactoryContext context)
    {
        var propertyProviders = context.Parameter.AttributeProvider.GetCustomAttributes(true)
            .OfType<IParameterPropertyProvider>();

        foreach (var propertyProvider in propertyProviders)
        {
            var properties = propertyProvider.UpdateParameterProperties(context);

            context = new ParameterFactoryContext(
                context.Parameter with { Properties = properties },
                context.ServiceProvider
            );
        }

        return context.Parameter with
        {
            Children = TransformParameters(context.Parameter.Children, context.ServiceProvider)
        };
    }
}
