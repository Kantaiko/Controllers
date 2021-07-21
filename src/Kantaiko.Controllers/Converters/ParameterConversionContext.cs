using System;
using System.Collections.Generic;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Converters
{
    public class ParameterConversionContext
    {
        public ParameterConversionContext(EndpointParameterInfo info, IReadOnlyDictionary<string, string> parameters,
            IServiceProvider serviceProvider)
        {
            Info = info;
            Parameters = parameters;
            ServiceProvider = serviceProvider;
        }

        public EndpointParameterInfo Info { get; }
        public IReadOnlyDictionary<string, string> Parameters { get; }
        public IServiceProvider ServiceProvider { get; }
    }
}
