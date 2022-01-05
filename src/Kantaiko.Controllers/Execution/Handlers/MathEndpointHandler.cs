using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Matching;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution.Handlers;

public class MathEndpointHandler<TContext> : ControllerExecutionHandler<TContext> where TContext : IContext
{
    protected override async Task<ControllerExecutionResult> HandleAsync(ControllerExecutionContext<TContext> context,
        NextAction next)
    {
        PropertyNullException.ThrowIfNull(context.IntrospectionInfo);

        foreach (var controller in context.IntrospectionInfo.Controllers)
        {
            foreach (var endpoint in controller.Endpoints)
            {
                var matchers = MatchingEndpointProperties<TContext>.Of(endpoint)?.EndpointMatchers;
                if (matchers is null) continue;

                var matchContext = new EndpointMatchContext<TContext>(context.RequestContext,
                    endpoint, context.ServiceProvider);

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

        return ControllerExecutionResult.NotMatched;
    }
}
