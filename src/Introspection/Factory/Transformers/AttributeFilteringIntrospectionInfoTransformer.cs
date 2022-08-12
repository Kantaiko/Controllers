using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;

namespace Kantaiko.Controllers.Introspection.Factory.Transformers;

public class AttributeFilteringIntrospectionInfoTransformer : IntrospectionInfoTransformer
{
    private readonly bool _requireControllerFilter;
    private readonly bool _requireEndpointFilter;

    public AttributeFilteringIntrospectionInfoTransformer(
        bool requireControllerFilter = false,
        bool requireEndpointFilter = false)
    {
        _requireControllerFilter = requireControllerFilter;
        _requireEndpointFilter = requireEndpointFilter;
    }

    protected override IImmutableList<ControllerInfo> TransformControllers(IntrospectionFactoryContext context)
    {
        var controllers = new List<ControllerInfo>(context.IntrospectionInfo.Controllers.Count);

        foreach (var controller in context.IntrospectionInfo.Controllers)
        {
            var filter = controller.Type.GetCustomAttributes(true).OfType<IControllerFilter>().FirstOrDefault();

            if (filter is not null)
            {
                var factoryContext = new ControllerFactoryContext(controller, context.ServiceProvider);
                if (!filter.IsIncluded(factoryContext)) continue;
            }
            else if (_requireControllerFilter)
            {
                continue;
            }

            controllers.Add(controller);
        }

        return controllers.ToImmutableArray();
    }

    protected override IImmutableList<EndpointInfo> TransformEndpoints(ControllerFactoryContext context)
    {
        var endpoints = new List<EndpointInfo>(context.Controller.Endpoints.Count);

        foreach (var endpoint in context.Controller.Endpoints)
        {
            var filter = endpoint.MethodInfo.GetCustomAttributes(true).OfType<IEndpointFilter>().FirstOrDefault();

            if (filter is not null)
            {
                var factoryContext = new EndpointFactoryContext(endpoint, context.ServiceProvider);
                if (!filter.IsIncluded(factoryContext)) continue;
            }
            else if (_requireEndpointFilter)
            {
                continue;
            }

            endpoints.Add(endpoint);
        }

        return endpoints.ToImmutableArray();
    }
}
