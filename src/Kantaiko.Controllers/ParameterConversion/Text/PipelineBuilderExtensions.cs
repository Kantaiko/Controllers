using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.ParameterConversion.Handlers;
using Kantaiko.Controllers.ParameterConversion.Text.Handlers;
using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.ParameterConversion.Text;

public static class PipelineBuilderExtensions
{
    public static void AddTextParameterConversion<TContext>(this PipelineBuilder<TContext> builder,
        IHandlerFactory? handlerFactory = null)
        where TContext : IContext
    {
        builder.AddParameterConversion(new ParameterConversionHandler<TContext>[]
        {
            new ResolveServiceParameterHandler<TContext>(),
            new CreateTextParameterConverterHandler<TContext>(handlerFactory),
            new CheckTextParameterValueExistenceHandler<TContext>(),
            new ValidateTextParameterValueHandler<TContext>(),
            new ResolveTextParameterValueHandler<TContext>(),
            new ResolveDefaultParameterValueHandler<TContext>(),
            new ReportMissingRequiredParameterHandler<TContext>(),
            new PostValidateParameterHandler<TContext>()
        });
    }
}
