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
        var controllers = context.IntrospectionInfo.Controllers;
        var result = new ControllerInfo[context.IntrospectionInfo.Controllers.Count];

        for (var index = 0; index < controllers.Count; index++)
        {
            var factoryContext = new ControllerFactoryContext(controllers[index], context.ServiceProvider);

            result[index] = TransformController(factoryContext);
        }

        return result.ToImmutableArray();
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
        var endpoints = context.Controller.Endpoints;
        var result = new EndpointInfo[endpoints.Count];

        for (var index = 0; index < endpoints.Count; index++)
        {
            var factoryContext = new EndpointFactoryContext(endpoints[index], context.ServiceProvider);

            result[index] = TransformEndpoint(factoryContext);
        }

        return result.ToImmutableArray();
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
        IReadOnlyList<EndpointParameterInfo> parameters,
        IServiceProvider serviceProvider)
    {
        var result = new EndpointParameterInfo[parameters.Count];

        for (var index = 0; index < parameters.Count; index++)
        {
            var factoryContext = new ParameterFactoryContext(parameters[index], serviceProvider);

            result[index] = TransformParameter(factoryContext);
        }

        return result.ToImmutableArray();
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
