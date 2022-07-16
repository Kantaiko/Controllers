using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Matching;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution.Handlers;

public class MatchEndpointHandler<TContext> : ControllerExecutionHandler<TContext>
{
    protected override async Task<ControllerResult> HandleAsync(ControllerContext<TContext> context, NextAction next)
    {
        PropertyNullException.ThrowIfNull(context.IntrospectionInfo);

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

                        return await next();
                    }
                }
            }
        }

        return ControllerResult.NotMatched;
    }
}
