using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Kantaiko.Controllers.Introspection.Factory.Context;

namespace Kantaiko.Controllers.Introspection.Factory.Transformers;

public class VisibilityFilteringIntrospectionInfoTransformer : IntrospectionInfoTransformer
{
    private readonly bool _requirePublic;
    private readonly bool _allowInheritance;

    public VisibilityFilteringIntrospectionInfoTransformer(bool requirePublic = true, bool allowInheritance = true)
    {
        _requirePublic = requirePublic;
        _allowInheritance = allowInheritance;
    }

    protected override IReadOnlyList<EndpointInfo> TransformEndpoints(ControllerFactoryContext context)
    {
        IEnumerable<EndpointInfo> endpoints = context.Controller.Endpoints;

        if (_requirePublic)
        {
            endpoints = endpoints.Where(x => x.MethodInfo.IsPublic);
        }

        if (!_allowInheritance)
        {
            endpoints = endpoints.Where(x => x.MethodInfo.GetBaseDefinition() == x.MethodInfo.DeclaringType);
        }

        return endpoints.ToImmutableArray();
    }
}
