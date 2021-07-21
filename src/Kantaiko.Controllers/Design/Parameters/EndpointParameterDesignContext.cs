using System;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Design.Parameters
{
    public class EndpointParameterDesignContext
    {
        public EndpointParameterDesignContext(EndpointParameterInfo info, IServiceProvider serviceProvider)
        {
            Info = info;
            ServiceProvider = serviceProvider;
        }

        public EndpointParameterInfo Info { get; }
        public IServiceProvider ServiceProvider { get; }
    }
}
