using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Controllers.Introspection.Contracts;

namespace Kantaiko.Controllers.Introspection.Transformers;

/// <summary>
/// The transformer that updates properties of controllers, endpoints and parameters
/// using <see cref="IControllerPropertyProvider"/>, <see cref="IEndpointPropertyProvider"/>
/// and <see cref="IParameterPropertyProvider"/> attributes.
/// </summary>
public sealed class PropertyProviderTransformer : IntrospectionInfoTransformer
{
    protected override ControllerInfo TransformController(ControllerTransformationContext context)
    {
        var propertyProviders = context.Controller.Type.GetCustomAttributes(true)
            .OfType<IControllerPropertyProvider>();

        foreach (var propertyProvider in propertyProviders)
        {
            var properties = propertyProvider.UpdateControllerProperties(context);

            context = new ControllerTransformationContext(
                context.Controller with { Properties = properties },
                context.ServiceProvider
            );
        }

        return context.Controller with { Endpoints = TransformEndpoints(context) };
    }

    protected override EndpointInfo TransformEndpoint(EndpointTransformationContext context)
    {
        var propertyProviders = context.Endpoint.MethodInfo.GetCustomAttributes(true)
            .OfType<IEndpointPropertyProvider>();

        foreach (var propertyProvider in propertyProviders)
        {
            var properties = propertyProvider.UpdateEndpointProperties(context);

            context = new EndpointTransformationContext(
                context.Endpoint with { Properties = properties },
                context.ServiceProvider
            );
        }

        return context.Endpoint with { ParameterTree = TransformParameterTree(context) };
    }

    protected override EndpointParameterInfo TransformParameter(ParameterTransformationContext context)
    {
        var propertyProviders = context.Parameter.AttributeProvider.GetCustomAttributes(true)
            .OfType<IParameterPropertyProvider>();

        foreach (var propertyProvider in propertyProviders)
        {
            var properties = propertyProvider.UpdateParameterProperties(context);

            context = new ParameterTransformationContext(
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
