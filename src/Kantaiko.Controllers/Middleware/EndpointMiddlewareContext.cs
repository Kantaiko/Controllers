using System;

namespace Kantaiko.Controllers.Middleware
{
    public class EndpointMiddlewareContext<TContext>
    {
        public EndpointMiddlewareContext(TContext requestContext, RequestExecutionContext<TContext> executionContext,
            IServiceProvider serviceProvider)
        {
            RequestContext = requestContext;
            ExecutionContext = executionContext;
            ServiceProvider = serviceProvider;
        }

        public EndpointMiddlewareStage Stage { get; internal set; } = EndpointMiddlewareStage.BeforeValidation;

        public TContext RequestContext { get; }
        public RequestExecutionContext<TContext> ExecutionContext { get; }

        public IServiceProvider ServiceProvider { get; }
        public bool ShouldProcess { get; set; } = true;
    }
}
