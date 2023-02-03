using System.Collections.Immutable;
using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Controllers.Introspection.Contracts;

namespace Kantaiko.Controllers.Introspection.Transformers;

/// <summary>
/// The transformer that filters out controllers and endpoints based on the
/// <see cref="IControllerFilter"/> and <see cref="IEndpointFilter"/> attributes.
/// </summary>
public sealed class AttributeFilteringTransformer : IntrospectionInfoTransformer
{
    private readonly bool _requireControllerFilter;
    private readonly bool _requireEndpointFilter;

    /// <summary>
    /// Creates a new instance of the <see cref="AttributeFilteringTransformer"/> class.
    /// </summary>
    /// <param name="requireControllerFilter">
    /// Whether to require the <see cref="IControllerFilter"/> attribute on controllers.
    /// By default, the attribute is not required and all controllers are included.
    /// </param>
    /// <param name="requireEndpointFilter">
    /// Whether to require the <see cref="IEndpointFilter"/> attribute on endpoints.
    /// By default, the attribute is not required and all endpoints are included.
    /// </param>
    public AttributeFilteringTransformer(bool requireControllerFilter = false, bool requireEndpointFilter = false)
    {
        _requireControllerFilter = requireControllerFilter;
        _requireEndpointFilter = requireEndpointFilter;
    }

    protected override IImmutableList<ControllerInfo> TransformControllers(IntrospectionTransformationContext context)
    {
        var controllers = new List<ControllerInfo>(context.IntrospectionInfo.Controllers.Count);

        foreach (var controller in context.IntrospectionInfo.Controllers)
        {
            var filter = controller.Type.GetCustomAttributes(true).OfType<IControllerFilter>().FirstOrDefault();

            if (filter is not null)
            {
                var factoryContext = new ControllerTransformationContext(controller, context.ServiceProvider);
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

    protected override IImmutableList<EndpointInfo> TransformEndpoints(ControllerTransformationContext context)
    {
        var endpoints = new List<EndpointInfo>(context.Controller.Endpoints.Count);

        foreach (var endpoint in context.Controller.Endpoints)
        {
            var filter = endpoint.MethodInfo.GetCustomAttributes(true).OfType<IEndpointFilter>().FirstOrDefault();

            if (filter is not null)
            {
                var factoryContext = new EndpointTransformationContext(endpoint, context.ServiceProvider);
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
