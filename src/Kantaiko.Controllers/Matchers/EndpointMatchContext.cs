using System;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Matchers
{
    public class EndpointMatchContext<TContext>
    {
        public EndpointMatchContext(TContext requestContext, EndpointInfo endpoint, IServiceProvider serviceProvider)
        {
            RequestContext = requestContext;
            Endpoint = endpoint;
            ServiceProvider = serviceProvider;
        }

        public TContext RequestContext { get; }
        public EndpointInfo Endpoint { get; }
        public IServiceProvider ServiceProvider { get; }
    }
}
