using System;
using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Routing.Abstractions;

namespace Kantaiko.Controllers.Execution;

public static class HandlerCollectionExtensions
{
    public static void AddEndpointMatching<TContext>(this IHandlerCollection<TContext> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        handlers.Add(new MatchEndpointHandler<TContext>());
    }

    public static void AddSubHandlerExecution<TContext>(this IHandlerCollection<TContext> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        handlers.Add(new ExecuteControllerSubHandlersHandler<TContext>());
        handlers.Add(new ExecuteEndpointSubHandlersHandler<TContext>());
    }

    public static void AddControllerInstantiation<TContext>(this IHandlerCollection<TContext> handlers,
        IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        handlers.Add(new InstantiateControllerHandler<TContext>(handlerFactory));
    }

    public static void AddEndpointInvocation<TContext>(this IHandlerCollection<TContext> handlers,
        bool awaitAsyncResult = true)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        handlers.Add(new InvokeEndpointHandler<TContext>());

        if (awaitAsyncResult)
        {
            handlers.Add(new AwaitAsyncResultHandler<TContext>());
        }
    }

    public static void AddRequestCompletion<TContext>(this IHandlerCollection<TContext> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        handlers.Add(new CompleteRequestHandler<TContext>());
    }

    public static void AddDefaultControllerHandling<TContext>(
        this IHandlerCollection<TContext> handlers,
        IHandlerFactory? handlerFactory = null,
        bool awaitAsyncResult = true)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        handlers.AddControllerInstantiation(handlerFactory);
        handlers.AddEndpointInvocation(awaitAsyncResult);
        handlers.AddRequestCompletion();
    }
}
