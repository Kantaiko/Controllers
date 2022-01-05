using System;

namespace Kantaiko.Controllers.Introspection.Factory.Context;

public readonly ref struct ParameterFactoryContext
{
    public ParameterFactoryContext(EndpointParameterInfo parameter, IServiceProvider serviceProvider)
    {
        Parameter = parameter;
        ServiceProvider = serviceProvider;
    }

    public EndpointParameterInfo Parameter { get; }
    public IServiceProvider ServiceProvider { get; }
}
