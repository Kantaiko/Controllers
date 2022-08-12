using Kantaiko.Controllers.Handlers;
using Kantaiko.Controllers.ParameterConversion.Text.Handlers;

namespace Kantaiko.Controllers.ParameterConversion.Text;

public static class ParameterHandlerCollectionExtensions
{
    public static void AddTextParameterConversion<TContext>(this IParameterHandlerCollection<TContext> builder,
        IHandlerFactory? handlerFactory = null)
    {
        builder.AddServiceParameterResolution();

        builder.Add(new CreateTextParameterConverterHandler<TContext>(handlerFactory));
        builder.Add(new CheckTextParameterValueExistenceHandler<TContext>());
        builder.Add(new ValidateTextParameterValueHandler<TContext>());
        builder.Add(new ResolveTextParameterValueHandler<TContext>());

        builder.AddDefaultValueResolution();
        builder.AddMissingParameterReporting();
        builder.AddPostValidation();
    }
}
