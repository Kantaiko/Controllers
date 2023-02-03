using System.Diagnostics;
using System.Runtime.CompilerServices;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.Utils;

internal static class Check
{
    public static TValue ContextProperty<TValue>(TValue? value, string handlerName,
        [CallerArgumentExpression("value")] string propertyName = default!)
    {
        if (value is null)
        {
            throw new InvalidOperationException(string.Format(Strings.RequiredContextPropertyNotSet,
                propertyName, handlerName));
        }

        return value;
    }

    public static object?[] ResolvedParameters(ControllerExecutionContext context)
    {
        Debug.Assert(context.Endpoint is not null, "Handler should check it explicitly");

        if (context.ResolvedParameters.Length != context.Endpoint.Parameters.Count)
        {
            throw new InvalidOperationException(string.Format(Strings.ResolvedParameterNumberMismatch,
                context.ResolvedParameters.Length, context.Endpoint.Parameters.Count));
        }

        return context.ResolvedParameters;
    }
}
