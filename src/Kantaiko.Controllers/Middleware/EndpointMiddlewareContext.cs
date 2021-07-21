using System;

namespace Kantaiko.Controllers.Middleware
{
    public class EndpointMiddlewareContext<TRequest>
    {
        public EndpointMiddlewareContext(TRequest request, RequestExecutionContext<TRequest> executionContext,
            IServiceProvider serviceProvider)
        {
            Request = request;
            ExecutionContext = executionContext;
            ServiceProvider = serviceProvider;
        }

        public EndpointMiddlewareStage Stage { get; internal set; } = EndpointMiddlewareStage.BeforeValidation;

        public TRequest Request { get; }
        public RequestExecutionContext<TRequest> ExecutionContext { get; }
        public IServiceProvider ServiceProvider { get; }
        public bool ShouldProcess { get; set; } = true;
    }
}
