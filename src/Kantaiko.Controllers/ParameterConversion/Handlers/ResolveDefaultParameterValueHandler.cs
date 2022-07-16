using System.Threading.Tasks;
using Kantaiko.Controllers.ParameterConversion.DefaultValue;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public class ResolveDefaultParameterValueHandler<TContext> : ParameterConversionHandler<TContext>
{
    protected override async Task HandleAsync(ParameterConversionContext<TContext> context)
    {
        if (context.ValueExists || context.ValueResolved)
        {
            return;
        }

        if (DefaultValueResolutionProperties.Of(context.Parameter) is not { DefaultValueResolver: { } resolver })
        {
            return;
        }

        var resolutionContext = new ParameterDefaultValueResolutionContext(context.Parameter,
            context.Context.ParameterConversionProperties, context.ServiceProvider);

        var value = await resolver.ResolveDefaultValueAsync(resolutionContext);

        context.ValueResolved = true;
        context.ResolvedValue = value;
    }
}
