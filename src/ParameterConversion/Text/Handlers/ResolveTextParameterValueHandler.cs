using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.ParameterConversion.Handlers;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.ParameterConversion.Text.Handlers;

public class ResolveTextParameterValueHandler<TContext> : IParameterConversionHandler<TContext>
{
    public async Task HandleAsync(ParameterConversionContext<TContext> context)
    {
        var properties = context.Properties.GetOrCreate<TextParameterConversionProperties>();

        PropertyNullException.ThrowIfNull(properties.Converter);
        PropertyNullException.ThrowIfNull(properties.ConversionContext);

        if (!context.ValueExists)
        {
            return;
        }

        var resolutionResult = await properties.Converter.ResolveAsync(properties.ConversionContext);

        if (!resolutionResult.Success)
        {
            context.ExecutionContext.ExecutionResult = ControllerExecutionResult.Error(resolutionResult.ErrorMessage);
            return;
        }

        context.ResolvedValue = resolutionResult.Value;
        context.ValueResolved = true;
    }
}
