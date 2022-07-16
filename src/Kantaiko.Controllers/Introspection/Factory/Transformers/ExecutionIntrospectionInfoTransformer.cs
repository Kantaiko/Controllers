using System.Linq;
using System.Reflection;
using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Execution.Properties;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Properties.Immutable;
using Kantaiko.Routing;

namespace Kantaiko.Controllers.Introspection.Factory.Transformers;

public class ExecutionIntrospectionInfoTransformer<TContext> : IntrospectionInfoTransformer
{
    protected override IImmutablePropertyCollection TransformControllerProperties(ControllerFactoryContext context)
    {
        var factories = context.Controller.Type.GetCustomAttributes()
            .OfType<IControllerExecutionHandlerFactory<TContext>>()
            .ToArray();

        if (factories.Length == 0)
        {
            return context.Controller.Properties;
        }

        var handlers = new IControllerExecutionHandler<TContext>[factories.Length];

        for (var index = 0; index < factories.Length; index++)
        {
            handlers[index] = factories[index].CreateHandler(context);
        }

        return context.Controller.Properties.Update<ControllerExecutionProperties<TContext>>(props =>
        {
            if (props.Handler is not null)
            {
                return props with { Handler = Handler.Chain(handlers.Prepend(props.Handler)) };
            }

            return props with { Handler = Handler.Chain(handlers) };
        });
    }

    protected override IImmutablePropertyCollection TransformEndpointProperties(EndpointFactoryContext context)
    {
        var attributes = context.Endpoint.MethodInfo.GetCustomAttributes(true);

        var factories = attributes
            .OfType<IEndpointExecutionHandlerFactory<TContext>>()
            .ToArray();

        if (factories.Length == 0)
        {
            return context.Endpoint.Properties;
        }

        var handlers = new IControllerExecutionHandler<TContext>[factories.Length];

        for (var index = 0; index < factories.Length; index++)
        {
            handlers[index] = factories[index].CreateHandler(context);
        }

        return context.Endpoint.Properties.Update<EndpointExecutionProperties<TContext>>(props =>
        {
            if (props.Handler is not null)
            {
                return props with { Handler = Handler.Chain(handlers.Prepend(props.Handler)) };
            }

            return props with { Handler = Handler.Chain(handlers) };
        });
    }
}
