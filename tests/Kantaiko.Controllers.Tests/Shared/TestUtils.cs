using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;

namespace Kantaiko.Controllers.Tests.Shared;

public static class TestUtils
{
    public static IHandler<TestContext, Task<ControllerExecutionResult>> CreateControllerHandler<T>(
        Action<IntrospectionBuilder<TestContext>>? configureIntrospection = null,
        Action<PipelineBuilder<TestContext>>? configurePipeline = null)
    {
        return CreateControllerHandler<T>(
            (introspectionBuilder, _) => configureIntrospection?.Invoke(introspectionBuilder),
            configurePipeline
        );
    }

    public static IHandler<TestContext, Task<ControllerExecutionResult>> CreateControllerHandler<T>(
        Action<IntrospectionBuilder<TestContext>, IReadOnlyList<Type>>? configureIntrospection = null,
        Action<PipelineBuilder<TestContext>>? configurePipeline = null)
    {
        var lookupTypes = typeof(T).GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);

        var introspectionBuilder = new IntrospectionBuilder<TestContext>();
        configureIntrospection?.Invoke(introspectionBuilder, lookupTypes);

        var introspectionInfo = introspectionBuilder.CreateIntrospectionInfo(lookupTypes);

        var pipelineBuilder = new PipelineBuilder<TestContext>();
        configurePipeline?.Invoke(pipelineBuilder);

        var handlers = pipelineBuilder.Build();

        return ControllerHandlerFactory.CreateControllerHandler(introspectionInfo, handlers);
    }
}
