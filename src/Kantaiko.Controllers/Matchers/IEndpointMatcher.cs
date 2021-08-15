namespace Kantaiko.Controllers.Matchers
{
    public interface IEndpointMatcher<TContext>
    {
        EndpointMatchResult Match(EndpointMatchContext<TContext> context);
    }
}
