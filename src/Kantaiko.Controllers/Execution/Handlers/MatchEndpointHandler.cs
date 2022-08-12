using System.Threading.Tasks;
using Kantaiko.Controllers.Matching;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution.Handlers;

public class MatchEndpointHandler<TContext> : IControllerExecutionHandler<TContext>
{
    public Task HandleAsync(ControllerExecutionContext<TContext> context)
    {
        foreach (var controller in context.IntrospectionInfo.Controllers)
        {
            foreach (var endpoint in controller.Endpoints)
            {
                if (MatchingEndpointProperties<TContext>.Of(endpoint) is not { EndpointMatchers: { } matchers })
                {
                    continue;
                }

                var matchContext = new EndpointMatchContext<TContext>(
                    context.RequestContext,
                    endpoint,
                    context.ServiceProvider
                );

                foreach (var matcher in matchers)
                {
                    var matchResult = matcher.Match(matchContext);

                    if (matchResult.IsSuccess)
                    {
                        context.Endpoint = endpoint;
                        context.ParameterConversionProperties = matchResult.Properties;

                        return Task.CompletedTask;
                    }
                }
            }
        }

        context.ExecutionResult = ControllerExecutionResult.NotMatched;
        return Task.CompletedTask;
    }
}
