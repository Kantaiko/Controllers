using System;
using System.Collections.Generic;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.ParameterConversion.Handlers;

namespace Kantaiko.Controllers.ParameterConversion;

public static class HandlerCollectionExtensions
{
    public static void AddParameterConversion<TContext>(
        this IHandlerCollection<TContext> handlers,
        IEnumerable<IParameterConversionHandler<TContext>> parameterHandlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        handlers.Add(new ConvertParametersHandler<TContext>(parameterHandlers));
        handlers.Add(new ConstructParametersHandler<TContext>());
    }

    public static void AddParameterConversion<TContext>(
        this IHandlerCollection<TContext> handlers,
        Action<IParameterHandlerCollection<TContext>> configureDelegate)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        var parameterHandlers = new ParameterHandlerCollection<TContext>();

        configureDelegate(parameterHandlers);

        handlers.AddParameterConversion(parameterHandlers);
    }
}
