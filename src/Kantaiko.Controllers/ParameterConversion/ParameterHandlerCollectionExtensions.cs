using System;
using Kantaiko.Controllers.ParameterConversion.Handlers;

namespace Kantaiko.Controllers.ParameterConversion;

public static class ParameterHandlerCollectionExtensions
{
    public static void AddServiceParameterResolution<TContext>(this IParameterHandlerCollection<TContext> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        handlers.Add(new ResolveServiceParameterHandler<TContext>());
    }

    public static void AddDefaultValueResolution<TContext>(this IParameterHandlerCollection<TContext> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        handlers.Add(new ResolveDefaultParameterValueHandler<TContext>());
    }

    public static void AddMissingParameterReporting<TContext>(this IParameterHandlerCollection<TContext> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        handlers.Add(new ReportMissingRequiredParameterHandler<TContext>());
    }

    public static void AddPostValidation<TContext>(this IParameterHandlerCollection<TContext> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        handlers.Add(new PostValidateParameterHandler<TContext>());
    }
}
