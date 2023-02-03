using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Controllers.Introspection.Transformers;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.EndpointMatching;

/// <summary>
/// The transformer that adds the attributes implementing <see cref="IEndpointMatcher"/> to the endpoint properties.
/// </summary>
public sealed class EndpointMatchingTransformer : IntrospectionInfoTransformer
{
    protected override IImmutablePropertyCollection TransformEndpointProperties(EndpointTransformationContext context)
    {
        var matchers = context.Endpoint.MethodInfo.GetCustomAttributes(true)
            .OfType<IEndpointMatcher>()
            .ToArray();

        return context.Endpoint.Properties.Set(new EndpointMatchingProperties(matchers));
    }
}
