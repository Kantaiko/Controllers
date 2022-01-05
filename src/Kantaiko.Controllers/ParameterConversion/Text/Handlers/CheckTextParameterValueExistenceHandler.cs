using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.ParameterConversion.Handlers;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.ParameterConversion.Text.Handlers;

public class CheckTextParameterValueExistenceHandler<TContext> : ParameterConversionHandler<TContext>
    where TContext : IContext
{
    protected override Task<Unit> HandleAsync(ParameterConversionContext<TContext> context)
    {
        var properties = context.Properties.GetOrCreate<TextParameterConversionProperties>();

        PropertyNullException.ThrowIfNull(properties.Converter);
        PropertyNullException.ThrowIfNull(properties.ConversionContext);

        context.ValueExists = properties.Converter.CheckValueExistence(properties.ConversionContext);

        return Unit.Task;
    }
}
