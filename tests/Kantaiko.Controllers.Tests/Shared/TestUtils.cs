using System;
using System.Collections.Generic;
using System.Reflection;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;

namespace Kantaiko.Controllers.Tests.Shared;

public static class TestUtils
{
    public static IControllerHandler<TestContext> CreateControllerHandler<T>(
        Action<IntrospectionBuilder<TestContext>>? configureIntrospection = null,
        Action<IHandlerCollection<TestContext>>? configurePipeline = null)
    {
        return CreateControllerHandler<T>(
            (introspectionBuilder, _) => configureIntrospection?.Invoke(introspectionBuilder),
            configurePipeline
        );
    }

    public static IControllerHandler<TestContext> CreateControllerHandler<T>(
        Action<IntrospectionBuilder<TestContext>, IReadOnlyList<Type>>? configureIntrospection = null,
        Action<IHandlerCollection<TestContext>>? configurePipeline = null)
    {
        var lookupTypes = typeof(T).GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);

        var introspectionBuilder = new IntrospectionBuilder<TestContext>();
        configureIntrospection?.Invoke(introspectionBuilder, lookupTypes);

        var introspectionInfo = introspectionBuilder.CreateIntrospectionInfo(lookupTypes);

        var handlers = new HandlerCollection<TestContext>();
        configurePipeline?.Invoke(handlers);

        return ControllerHandlerFactory.CreateControllerHandler(introspectionInfo, handlers);
    }
}
