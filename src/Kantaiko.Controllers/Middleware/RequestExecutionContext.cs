using System.Collections.Generic;
using Kantaiko.Controllers.Internal;

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

        public IReadOnlyDictionary<string, ExecutionParameterContext> Parameters { get; }
        internal EndpointManager<TRequest> EndpointManager { get; }
    }
}
