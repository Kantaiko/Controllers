using System;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.ParameterConversion.DefaultValue;

public class ParameterDefaultValueResolutionContext
{
    public ParameterDefaultValueResolutionContext(
        EndpointParameterInfo parameter,
        IImmutablePropertyCollection? parameterConversionProperties,
        IServiceProvider serviceProvider)
    {
        Parameter = parameter;
        ParameterConversionProperties = parameterConversionProperties;
        ServiceProvider = serviceProvider;
    }

    public EndpointParameterInfo Parameter { get; }
    public IImmutablePropertyCollection? ParameterConversionProperties { get; }
    public IServiceProvider ServiceProvider { get; }
}
