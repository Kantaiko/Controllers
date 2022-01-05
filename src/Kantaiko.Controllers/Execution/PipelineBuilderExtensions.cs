using System.Collections.Generic;
using System.Threading.Tasks;
using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;
using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.Execution;

using CResult = Task<ControllerExecutionResult>;

public static class PipelineBuilderExtensions
{
    public static void AddEndpointMatching<TContext>(this PipelineBuilder<TContext> builder)
        where TContext : IContext
    {
        builder.AddHandler(new MathEndpointHandler<TContext>());
    }

    public static void AddSubHandlerExecution<TContext>(this PipelineBuilder<TContext> builder)
        where TContext : IContext
    {
        builder.AddHandler(new ExecuteControllerSubHandlersHandler<TContext>());
        builder.AddHandler(new ExecuteEndpointSubHandlersHandler<TContext>());
    }

    public static void AddControllerInstantiation<TContext>(this PipelineBuilder<TContext> builder,
        IHandlerFactory? handlerFactory = null)
        where TContext : IContext
    {
        builder.AddHandler(new InstantiateControllerHandler<TContext>(handlerFactory));
    }

    public static void AddEndpointInvocation<TContext>(this PipelineBuilder<TContext> builder,
        bool awaitAsyncResult = true)
        where TContext : IContext
    {
        builder.AddHandler(new InvokeEndpointHandler<TContext>());

        if (awaitAsyncResult)
        {
            builder.AddHandler(new AwaitAsyncResultHandler<TContext>());
        }
    }

    public static void AddRequestCompletion<TContext>(this PipelineBuilder<TContext> builder)
        where TContext : IContext
    {
        builder.AddHandler(new CompleteRequestHandler<TContext>());
    }

    public static void AddDefaultControllerHandling<TContext>(this PipelineBuilder<TContext> builder,
        IHandlerFactory? handlerFactory = null,
        bool awaitAsyncResult = true) where TContext : IContext
    {
        builder.AddControllerInstantiation(handlerFactory);
        builder.AddEndpointInvocation(awaitAsyncResult);
        builder.AddRequestCompletion();
    }

    public static void AddParameterConversion<TContext>(this PipelineBuilder<TContext> builder,
        IEnumerable<IHandler<ParameterConversionContext<TContext>, Task<Unit>>> handlers)
        where TContext : IContext
    {
        builder.AddHandler(new ConvertParametersHandler<TContext>(handlers));
        builder.AddHandler(new ConstructParametersHandler<TContext>());
    }
}
