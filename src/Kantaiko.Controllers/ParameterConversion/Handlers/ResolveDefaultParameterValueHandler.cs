using System.Threading.Tasks;
using Kantaiko.Controllers.ParameterConversion.DefaultValue;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public class ResolveDefaultParameterValueHandler<TContext> : ParameterConversionHandler<TContext>
    where TContext : IContext
{
    protected override async Task<Unit> HandleAsync(ParameterConversionContext<TContext> context)
    {
        if (context.ValueExists || context.ValueResolved)
        {
            return default;
        }

        if (DefaultValueResolutionParameterProperties.Of(context.Parameter)?.DefaultValueResolver is not { } resolver)
        {
            return default;
        }

        var resolutionContext = new ParameterDefaultValueResolutionContext(context.Parameter,
            context.ExecutionContext.ParameterConversionProperties, context.ServiceProvider);

        var value = await resolver.ResolveDefaultValueAsync(resolutionContext);

        context.ValueResolved = true;
        context.ResolvedValue = value;

        return default;
    }
}
