namespace Kantaiko.Controllers.Matchers
{
    public interface IEndpointMatcher<TRequest>
    {
        EndpointMatchResult Match(EndpointMatchContext<TRequest> context);
    }
}
