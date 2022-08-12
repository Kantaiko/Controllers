using System;

namespace Kantaiko.Controllers.Introspection.Factory.Context;

public readonly ref struct IntrospectionFactoryContext
{
    public IntrospectionFactoryContext(IntrospectionInfo introspectionInfo, IServiceProvider serviceProvider)
    {
        IntrospectionInfo = introspectionInfo;
        ServiceProvider = serviceProvider;
    }

    public IntrospectionInfo IntrospectionInfo { get; }
    public IServiceProvider ServiceProvider { get; }
}
