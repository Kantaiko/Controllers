using System;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Validation
{
    public class ParameterPostValidationContext
    {
        public ParameterPostValidationContext(EndpointParameterInfo info, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Info = info;
        }

        public EndpointParameterInfo Info { get; }
        public IServiceProvider ServiceProvider { get; }
    }
}
