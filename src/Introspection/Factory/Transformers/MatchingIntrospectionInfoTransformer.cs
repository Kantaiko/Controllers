using System.Linq;
using System.Reflection;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.Matching;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection.Factory.Transformers;

public class MatchingIntrospectionInfoTransformer<TContext> : IntrospectionInfoTransformer
{
    protected override IImmutablePropertyCollection TransformEndpointProperties(EndpointFactoryContext context)
    {
        var factories = context.Endpoint.MethodInfo.GetCustomAttributes()
            .OfType<IEndpointMatcherFactory<TContext>>()
            .ToArray();

        var matchers = new IEndpointMatcher<TContext>[factories.Length];

        for (var index = 0; index < factories.Length; index++)
        {
            matchers[index] = factories[index].CreateEndpointMatcher(context);
        }

        return context.Endpoint.Properties.Update<MatchingEndpointProperties<TContext>>(props => props with
        {
            EndpointMatchers = props.EndpointMatchers.Concat(matchers)
        });
    }
}
