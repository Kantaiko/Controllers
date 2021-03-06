using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.ParameterConversion.Handlers;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;

namespace Kantaiko.Controllers.ParameterConversion.Text.Handlers;

public class ResolveTextParameterValueHandler<TContext> : ParameterConversionHandler<TContext>
{
    protected override async Task<Unit> HandleAsync(ParameterConversionContext<TContext> context)
    {
        var properties = context.Properties.GetOrCreate<TextParameterConversionProperties>();

        PropertyNullException.ThrowIfNull(properties.Converter);
        PropertyNullException.ThrowIfNull(properties.ConversionContext);

        if (!context.ValueExists)
        {
            return default;
        }

        var resolutionResult = await properties.Converter.ResolveAsync(properties.ConversionContext);

        if (!resolutionResult.Success)
        {
            context.ExecutionResult = ControllerResult.Error(resolutionResult.ErrorMessage);
            return default;
        }

        context.ResolvedValue = resolutionResult.Value;
        context.ValueResolved = true;

        return default;
    }
}
