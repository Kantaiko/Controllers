using System;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Design.Endpoints
{
    public class EndpointDesignContext
    {
        public EndpointDesignContext(EndpointInfo info, IServiceProvider serviceProvider)
        {
            Info = info;
            ServiceProvider = serviceProvider;
        }

        public EndpointInfo Info { get; }
        public IServiceProvider ServiceProvider { get; }
    }
}
