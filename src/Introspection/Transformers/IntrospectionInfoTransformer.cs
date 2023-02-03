using System.Collections.Immutable;
using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection.Transformers;

/// <summary>
/// The base class for introspection transformers providing a convenient way to transform controllers,
/// endpoints and parameters.
/// </summary>
public abstract class IntrospectionInfoTransformer : IIntrospectionInfoTransformer
{
    public IntrospectionInfo Transform(IntrospectionInfo introspectionInfo, IServiceProvider serviceProvider)
    {
        var factoryContext = new IntrospectionTransformationContext(introspectionInfo, serviceProvider);
        return Transform(factoryContext);
    }

    /// <summary>
    /// Transforms the introspection info.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>The transformed introspection info or the same instance if no changes were made.</returns>
    protected virtual IntrospectionInfo Transform(IntrospectionTransformationContext context)
    {
        return context.IntrospectionInfo with { Controllers = TransformControllers(context) };
    }

    /// <summary>
    /// Transforms the list of controllers.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>The transformed list of controllers or the same instance if no changes were made.</returns>
    protected virtual IReadOnlyList<ControllerInfo> TransformControllers(IntrospectionTransformationContext context)
    {
        var controllers = context.IntrospectionInfo.Controllers;
        var result = new ControllerInfo[context.IntrospectionInfo.Controllers.Count];

        for (var index = 0; index < controllers.Count; index++)
        {
            var factoryContext = new ControllerTransformationContext(controllers[index], context.ServiceProvider);

            result[index] = TransformController(factoryContext);
        }

        return result.ToImmutableArray();
    }

    /// <summary>
    /// Transforms a controller.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>The transformed controller or the same instance if no changes were made.</returns>
    protected virtual ControllerInfo TransformController(ControllerTransformationContext context)
    {
        return context.Controller with
        {
            Endpoints = TransformEndpoints(context),
            Properties = TransformControllerProperties(context)
        };
    }

    /// <summary>
    /// Transforms the list of endpoints.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>The transformed list of endpoints or the same instance if no changes were made.</returns>
    protected virtual IReadOnlyList<EndpointInfo> TransformEndpoints(ControllerTransformationContext context)
    {
        var endpoints = context.Controller.Endpoints;
        var result = new EndpointInfo[endpoints.Count];

        for (var index = 0; index < endpoints.Count; index++)
        {
            var factoryContext = new EndpointTransformationContext(endpoints[index], context.ServiceProvider);

            result[index] = TransformEndpoint(factoryContext);
        }

        return result.ToImmutableArray();
    }

    /// <summary>
    /// Transforms controller properties.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>The transformed endpoint or the same instance if no changes were made.</returns>
    protected virtual IImmutablePropertyCollection TransformControllerProperties(
        ControllerTransformationContext context)
    {
        return context.Controller.Properties;
    }

    /// <summary>
    /// Transforms an endpoint.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>The transformed endpoint or the same instance if no changes were made.</returns>
    protected virtual EndpointInfo TransformEndpoint(EndpointTransformationContext context)
    {
        return context.Endpoint with
        {
            ParameterTree = TransformParameterTree(context),
            Properties = TransformEndpointProperties(context)
        };
    }

    /// <summary>
    /// Transforms the parameter tree.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>The transformed parameter tree or the same instance if no changes were made.</returns>
    protected virtual IReadOnlyList<EndpointParameterInfo> TransformParameterTree(
        EndpointTransformationContext context)
    {
        return TransformParameters(context.Endpoint.ParameterTree, context.ServiceProvider);
    }

    /// <summary>
    /// Transforms the list of parameters.
    /// </summary>
    /// <param name="parameters">The list of parameters to transform.</param>
    /// <param name="serviceProvider"></param>
    /// <returns>The transformed list of parameters or the same instance if no changes were made.</returns>
    protected virtual IReadOnlyList<EndpointParameterInfo> TransformParameters(
        IReadOnlyList<EndpointParameterInfo> parameters,
        IServiceProvider serviceProvider)
    {
        var result = new EndpointParameterInfo[parameters.Count];

        for (var index = 0; index < parameters.Count; index++)
        {
            var factoryContext = new ParameterTransformationContext(parameters[index], serviceProvider);

            result[index] = TransformParameter(factoryContext);
        }

        return result.ToImmutableArray();
    }

    /// <summary>
    /// Transforms endpoint properties.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>The transformed endpoint properties or the same instance if no changes were made.</returns>
    protected virtual IImmutablePropertyCollection TransformEndpointProperties(EndpointTransformationContext context)
    {
        return context.Endpoint.Properties;
    }

    /// <summary>
    /// Transforms a parameter.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>The transformed parameter or the same instance if no changes were made.</returns>
    protected virtual EndpointParameterInfo TransformParameter(ParameterTransformationContext context)
    {
        return context.Parameter with
        {
            Properties = TransformParameterProperties(context),
            Children = TransformParameters(context.Parameter.Children, context.ServiceProvider)
        };
    }

    /// <summary>
    /// Transforms a parameter properties.
    /// </summary>
    /// <param name="context">The transformation context.</param>
    /// <returns>The transformed parameter properties or the same instance if no changes were made.</returns>
    protected virtual IImmutablePropertyCollection TransformParameterProperties(ParameterTransformationContext context)
    {
        return context.Parameter.Properties;
    }
}
