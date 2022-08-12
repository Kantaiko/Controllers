using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.ParameterConversion.Handlers;

namespace Kantaiko.Controllers.ParameterConversion.Text.Handlers;

public class CheckTextParameterValueExistenceHandler<TContext> : IParameterConversionHandler<TContext>
{
    public Task HandleAsync(ParameterConversionContext<TContext> context)
    {
        var properties = context.Properties.GetOrCreate<TextParameterConversionProperties>();

        PropertyNullException.ThrowIfNull(properties.Converter);
        PropertyNullException.ThrowIfNull(properties.ConversionContext);

        context.ValueExists = properties.Converter.CheckValueExistence(properties.ConversionContext);

        return Task.CompletedTask;
    }
}
