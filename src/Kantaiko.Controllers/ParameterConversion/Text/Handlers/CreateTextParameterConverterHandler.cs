using System;
using System.Threading.Tasks;
using Kantaiko.Controllers.Matching.Text;
using Kantaiko.Controllers.ParameterConversion.Handlers;
using Kantaiko.Controllers.ParameterConversion.Text.Properties;
using Kantaiko.Properties;
using Kantaiko.Routing;
using Kantaiko.Routing.Abstractions;

namespace Kantaiko.Controllers.ParameterConversion.Text.Handlers;

public class CreateTextParameterConverterHandler<TContext> : ParameterConversionHandler<TContext>
{
    private readonly IHandlerFactory _handlerFactory;

    public CreateTextParameterConverterHandler(IHandlerFactory? handlerFactory = null)
    {
        _handlerFactory = handlerFactory ?? DefaultHandlerFactory.Instance;
    }

    protected override Task<Unit> HandleAsync(ParameterConversionContext<TContext> context)
    {
        if (TextConversionParameterProperties.Of(context.Parameter) is not { } parameterProperties)
        {
            throw new InvalidOperationException("Text parameter converter requires TextConversionParameterProperties");
        }

        if (parameterProperties is { ConverterFactory: null, ConverterType: null })
        {
            throw new InvalidOperationException("Text parameter converter requires ConverterFactory or ConverterType");
        }

        if (context.Context.ParameterConversionProperties is not { } parameterConversionProperties)
        {
            throw new InvalidOperationException("Text parameter converter requires ParameterConversionProperties");
        }

        var parameters = parameterConversionProperties.Get<MatchingTextParameterConversionProperties>()?.Parameters;

        if (parameters is null)
        {
            throw new InvalidOperationException(
                "Text parameter converter requires MatchingTextParameterConversionProperties.Parameters");
        }

        context.Configure<TextParameterConversionProperties>(properties =>
        {
            properties.Converter = parameterProperties.ConverterFactory is not null
                ? parameterProperties.ConverterFactory(context.ServiceProvider)
                : (ITextParameterConverter) _handlerFactory.CreateHandler(
                    parameterProperties.ConverterType!, context.ServiceProvider);

            properties.ConversionContext = new TextParameterConversionContext(parameters, context.Parameter);
        });

        return Unit.Task;
    }
}
