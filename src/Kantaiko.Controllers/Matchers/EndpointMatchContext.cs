using System;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Matchers
{
    public class EndpointMatchContext<TRequest>
    {
        public EndpointMatchContext(TRequest request, EndpointInfo endpoint, IServiceProvider serviceProvider)
        {
            Request = request;
            Endpoint = endpoint;
            ServiceProvider = serviceProvider;
        }

        public TRequest Request { get; }
        public EndpointInfo Endpoint { get; }
        public IServiceProvider ServiceProvider { get; }
    }
}
