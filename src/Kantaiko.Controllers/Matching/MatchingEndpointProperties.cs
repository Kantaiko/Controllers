using System.Collections.Generic;
using System.Linq;
using Kantaiko.Properties;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Matching;

public record MatchingEndpointProperties<TContext> : ReadOnlyPropertiesBase<MatchingEndpointProperties<TContext>>
    where TContext : IContext
{
    public IEnumerable<IEndpointMatcher<TContext>> EndpointMatchers { get; init; } =
        Enumerable.Empty<IEndpointMatcher<TContext>>();
}
