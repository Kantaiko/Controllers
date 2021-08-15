using System.Collections.Generic;
using Kantaiko.Controllers.Internal;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Middleware
{
    public class RequestExecutionContext<TContext>
    {
        internal RequestExecutionContext(IReadOnlyDictionary<string, ExecutionParameterContext> parameters,
            EndpointManager<TContext> endpointManager)
        {
            Parameters = parameters;
            EndpointManager = endpointManager;
        }

        public object? ControllerInstance { get; internal set; }
        public RequestProcessingResult? ProcessingResult { get; internal set; }

        public IReadOnlyDictionary<string, ExecutionParameterContext> Parameters { get; }
        internal EndpointManager<TContext> EndpointManager { get; }

        public EndpointInfo Endpoint => EndpointManager.Info;
    }
}
