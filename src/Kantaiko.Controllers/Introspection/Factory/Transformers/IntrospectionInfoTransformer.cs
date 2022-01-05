using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection.Factory.Transformers;

public abstract class IntrospectionInfoTransformer : IIntrospectionInfoTransformer
{
    public virtual IntrospectionInfo Transform(IntrospectionInfo introspectionInfo, IServiceProvider serviceProvider)
    {
        var factoryContext = new IntrospectionFactoryContext(introspectionInfo, serviceProvider);
        return Transform(factoryContext);
    }

    protected virtual IntrospectionInfo Transform(IntrospectionFactoryContext context)
    {
        return context.IntrospectionInfo with
        {
            Controllers = TransformControllers(context)
        };
    }

    protected virtual IReadOnlyList<ControllerInfo> TransformControllers(IntrospectionFactoryContext context)
    {
        var controllers = new ControllerInfo[context.IntrospectionInfo.Controllers.Count];

        for (var index = 0; index < context.IntrospectionInfo.Controllers.Count; index++)
        {
            var controller = context.IntrospectionInfo.Controllers[index];
            var factoryContext = new ControllerFactoryContext(controller, context.ServiceProvider);

            controllers[index] = TransformController(factoryContext);
        }

        return controllers.ToImmutableArray();
    }

    protected virtual ControllerInfo TransformController(ControllerFactoryContext context)
    {
        return context.Controller with
        {
            Endpoints = TransformEndpoints(context),
            Properties = TransformControllerProperties(context)
        };
    }

    protected virtual IReadOnlyList<EndpointInfo> TransformEndpoints(ControllerFactoryContext context)
    {
        var endpoints = new EndpointInfo[context.Controller.Endpoints.Count];

        for (var index = 0; index < context.Controller.Endpoints.Count; index++)
        {
            var endpoint = context.Controller.Endpoints[index];
            var factoryContext = new EndpointFactoryContext(endpoint, context.ServiceProvider);

            endpoints[index] = TransformEndpoint(factoryContext);
        }

        return endpoints.ToImmutableArray();
    }

    protected virtual IImmutablePropertyCollection TransformControllerProperties(ControllerFactoryContext context)
    {
        return context.Controller.Properties;
    }

    protected virtual EndpointInfo TransformEndpoint(EndpointFactoryContext context)
    {
        return context.Endpoint with
        {
            ParameterTree = TransformParameterTree(context),
            Properties = TransformEndpointProperties(context)
        };
    }

    protected virtual IReadOnlyList<EndpointParameterInfo> TransformParameterTree(
        EndpointFactoryContext context)
    {
        return TransformParameters(context.Endpoint.ParameterTree, context.ServiceProvider);
    }

    protected virtual IReadOnlyList<EndpointParameterInfo> TransformParameters(
        IReadOnlyList<EndpointParameterInfo> inputParameters,
        IServiceProvider serviceProvider)
    {
        var parameters = new EndpointParameterInfo[inputParameters.Count];

        for (var index = 0; index < inputParameters.Count; index++)
        {
            var parameter = inputParameters[index];
            var factoryContext = new ParameterFactoryContext(parameter, serviceProvider);

            parameters[index] = TransformParameter(factoryContext);
        }

        return parameters.ToImmutableArray();
    }

    protected virtual IImmutablePropertyCollection TransformEndpointProperties(EndpointFactoryContext context)
    {
        return context.Endpoint.Properties;
    }

    protected virtual EndpointParameterInfo TransformParameter(ParameterFactoryContext context)
    {
        return context.Parameter with
        {
            Properties = TransformParameterProperties(context),
            Children = TransformParameters(context.Parameter.Children, context.ServiceProvider)
        };
    }

    protected virtual IImmutablePropertyCollection TransformParameterProperties(ParameterFactoryContext context)
    {
        return context.Parameter.Properties;
    }
}
