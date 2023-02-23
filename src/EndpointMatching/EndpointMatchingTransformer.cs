using Kantaiko.Controllers.Introspection.Context;
using Kantaiko.Controllers.Introspection.Transformers;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.EndpointMatching;

/// <summary>
/// The transformer that adds the attributes implementing <see cref="IEndpointMatcher"/> to the endpoint properties.
/// </summary>
public sealed class EndpointMatchingTransformer : IntrospectionInfoTransformer
{
    private readonly bool _inheritMatchers;

    /// <summary>
    /// Creates a new instance of the <see cref="EndpointMatchingTransformer"/> class.
    /// </summary>
    /// <param name="inheritMatchers">
    /// Whether to inherit the endpoint matchers from the base class.
    /// By default, the matchers are not inherited.
    /// </param>
    public EndpointMatchingTransformer(bool inheritMatchers = false)
    {
        _inheritMatchers = inheritMatchers;
    }

    protected override IImmutablePropertyCollection TransformEndpointProperties(EndpointTransformationContext context)
    {
        var matchers = context.Endpoint.MethodInfo.GetCustomAttributes(_inheritMatchers)
            .OfType<IEndpointMatcher>()
            .ToArray();

        return context.Endpoint.Properties.Set(new EndpointMatchingProperties(matchers));
    }
}
