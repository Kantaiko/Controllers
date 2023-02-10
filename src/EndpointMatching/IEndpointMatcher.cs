using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.EndpointMatching;

/// <summary>
/// The interface for endpoint matchers.
/// </summary>
public interface IEndpointMatcher
{
    /// <summary>
    /// Tries to match the endpoint with the request context.
    /// </summary>
    /// <param name="context">The context of the endpoint matching.</param>
    /// <returns>The result of the endpoint matching.</returns>
    EndpointMatchingResult Match(EndpointMatchingContext context);
}

/// <summary>
/// The generic version of <see cref="IEndpointMatcher"/>.
/// </summary>
/// <typeparam name="TContext">The type of the request context.</typeparam>
public interface IEndpointMatcher<TContext> : IEndpointMatcher
{
    protected EndpointMatchingResult Match(EndpointMatchingContext<TContext> context);

    EndpointMatchingResult IEndpointMatcher.Match(EndpointMatchingContext context)
    {
        if (context.RequestContext is not TContext requestContext)
        {
            throw new InvalidOperationException(string.Format(Strings.InvalidMatcherContextType,
                context.RequestContext.GetType(), typeof(TContext)));
        }

        return Match(new EndpointMatchingContext<TContext>(requestContext, context.Endpoint, context.Properties,
            context.ServiceProvider));
    }
}
