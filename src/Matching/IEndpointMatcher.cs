namespace Kantaiko.Controllers.Matching;

public interface IEndpointMatcher<TContext>
{
    EndpointMatchResult Match(EndpointMatchContext<TContext> context);
}
