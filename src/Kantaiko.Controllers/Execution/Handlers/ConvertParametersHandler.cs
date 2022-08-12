using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Introspection;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.ParameterConversion.Handlers;

namespace Kantaiko.Controllers.Execution.Handlers;

public class ConvertParametersHandler<TContext> : IControllerExecutionHandler<TContext>
{
    private readonly IEnumerable<IParameterConversionHandler<TContext>> _handlers;

    public ConvertParametersHandler(IEnumerable<IParameterConversionHandler<TContext>> handlers)
    {
        _handlers = handlers;
    }

    public async Task HandleAsync(ControllerExecutionContext<TContext> context)
    {
        PropertyNullException.ThrowIfNull(context.Endpoint);

        var conversionContexts = context.Endpoint.Parameters
            .Select(x => new ParameterConversionContext<TContext>(context, x))
            .ToArray();

        context.ResolvedParameters = new Dictionary<EndpointParameterInfo, object?>(conversionContexts.Length);

        foreach (var handler in _handlers)
        {
            foreach (var conversionContext in conversionContexts)
            {
                await handler.HandleAsync(conversionContext);

                if (context.ExecutionResult is not null)
                {
                    return;
                }

                if (conversionContext.ValueResolved)
                {
                    context.ResolvedParameters[conversionContext.Parameter] = conversionContext.ResolvedValue;
                }
            }
        }
    }
}
