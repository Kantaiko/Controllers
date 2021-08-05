using System.Collections.Generic;
using Kantaiko.Controllers.Internal;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Middleware
{
    public class RequestExecutionContext<TRequest>
    {
        internal RequestExecutionContext(IReadOnlyDictionary<string, ExecutionParameterContext> parameters,
            EndpointManager<TRequest> endpointManager)
        {
            Parameters = parameters;
            EndpointManager = endpointManager;
        }

        public object? ControllerInstance { get; internal set; }
        public RequestProcessingResult? ProcessingResult { get; internal set; }

        public IReadOnlyDictionary<string, ExecutionParameterContext> Parameters { get; }
        internal EndpointManager<TRequest> EndpointManager { get; }
    }
}
