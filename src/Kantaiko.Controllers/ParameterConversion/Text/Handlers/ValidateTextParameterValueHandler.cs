using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.ParameterConversion.Handlers;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.ParameterConversion.Text.Handlers;

public class ValidateTextParameterValueHandler<TContext> : IParameterConversionHandler<TContext>
{
    public Task HandleAsync(ParameterConversionContext<TContext> context)
    {
        var properties = context.Properties.GetOrCreate<TextParameterConversionProperties>();

        PropertyNullException.ThrowIfNull(properties.Converter);
        PropertyNullException.ThrowIfNull(properties.ConversionContext);

        if (!context.ValueExists)
        {
            return Task.CompletedTask;
        }

        var validationResult = properties.Converter.Validate(properties.ConversionContext);

        if (!validationResult.IsValid)
        {
            context.ExecutionContext.ExecutionResult = ControllerExecutionResult.Error(validationResult.ErrorMessage);
        }

        return Task.CompletedTask;
    }
}
