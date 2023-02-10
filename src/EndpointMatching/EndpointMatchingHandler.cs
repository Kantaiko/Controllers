using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.EndpointMatching;

/// <summary>
/// The handler that matches an endpoint based on the request context and available endpoint matchers.
/// </summary>
public sealed class EndpointMatchingHandler : IControllerExecutionHandler
{
    Task IControllerExecutionHandler.HandleAsync(ControllerExecutionContext context)
    {
        foreach (var controller in context.IntrospectionInfo.Controllers)
        {
            foreach (var endpoint in controller.Endpoints)
            {
                if (EndpointMatchingProperties.Of(endpoint) is not { Matchers: { } matchers })
                {
                    continue;
                }

                var matchContext = new EndpointMatchingContext(context, endpoint);

                foreach (var matcher in matchers)
                {
                    EndpointMatchingResult matchingResult;

                    try
                    {
                        matchingResult = matcher.Match(matchContext);
                    }
                    catch (Exception exception)
                    {
                        context.ExecutionError = new ControllerError(ControllerErrorCodes.MatchingException)
                        {
                            Message = Strings.MatchingException,
                            Exception = exception,
                            Endpoint = endpoint
                        };

                        return Task.CompletedTask;
                    }

                    if (matchingResult.Error is not null)
                    {
                        context.ExecutionError = new ControllerError(ControllerErrorCodes.MatchingFailed)
                        {
                            Message = Strings.MatchingError,
                            InnerError = matchingResult.Error,
                            Endpoint = endpoint
                        };

                        return Task.CompletedTask;
                    }

                    if (matchingResult.Matched)
                    {
                        context.Endpoint = endpoint;
                        context.MatchProperties = matchingResult.MatchProperties;

                        return Task.CompletedTask;
                    }
                }
            }
        }

        context.ExecutionError = new ControllerError(ControllerErrorCodes.NotMatched)
        {
            Message = Strings.NotMatched
        };

        return Task.CompletedTask;
    }
}
