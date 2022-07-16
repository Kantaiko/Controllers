using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.ParameterConversion.Handlers;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;

namespace Kantaiko.Controllers.ParameterConversion.Text.Handlers;

public class ValidateTextParameterValueHandler<TContext> : ParameterConversionHandler<TContext>
{
    protected override Task<Unit> HandleAsync(ParameterConversionContext<TContext> context)
    {
        var properties = context.Properties.GetOrCreate<TextParameterConversionProperties>();

        PropertyNullException.ThrowIfNull(properties.Converter);
        PropertyNullException.ThrowIfNull(properties.ConversionContext);

        if (!context.ValueExists)
        {
            return Unit.Task;
        }

        var validationResult = properties.Converter.Validate(properties.ConversionContext);

        if (!validationResult.IsValid)
        {
            context.ExecutionResult = ControllerResult.Error(validationResult.ErrorMessage);
        }

        return Unit.Task;
    }
}
