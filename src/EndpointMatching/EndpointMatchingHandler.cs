using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.EndpointMatching;

/// <summary>
/// The handler that matches an endpoint based on the request context and available endpoint matchers.
/// </summary>
public sealed class EndpointMatchingHandler : IControllerExecutionHandler
{
    private readonly bool _immediatelyReturnError;

    /// <summary>
    /// Creates a new instance of the <see cref="EndpointMatchingHandler"/> class.
    /// </summary>
    /// <param name="immediatelyReturnError">
    /// Whether to immediately return an error returned by the endpoint matcher. Otherwise, the error will be returned
    /// only if no other endpoint matches the request.
    /// If there are multiple endpoints returning errors, the last error will be returned.
    /// <br/>
    /// By default, the error is returned immediately.
    /// </param>
    public EndpointMatchingHandler(bool immediatelyReturnError = true)
    {
        _immediatelyReturnError = immediatelyReturnError;
    }

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

                        if (_immediatelyReturnError)
                        {
                            return Task.CompletedTask;
                        }
                    }

                    if (matchingResult.Matched)
                    {
                        context.Endpoint = endpoint;
                        context.MatchProperties = matchingResult.MatchProperties;
                        context.ExecutionError = null;

                        return Task.CompletedTask;
                    }
                }
            }
        }

        context.ExecutionError ??= new ControllerError(ControllerErrorCodes.NotMatched)
        {
            Message = Strings.NotMatched
        };

        return Task.CompletedTask;
    }
}
