using System.Collections.Generic;
using System.Linq;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.Matching;

public record MatchingEndpointProperties<TContext> : ReadOnlyPropertiesBase<MatchingEndpointProperties<TContext>>
{
    public IEnumerable<IEndpointMatcher<TContext>> EndpointMatchers { get; init; } =
        Enumerable.Empty<IEndpointMatcher<TContext>>();
}
