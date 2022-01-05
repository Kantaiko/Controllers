using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.ParameterConversion.Properties;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public class ResolveServiceParameterHandler<TContext> : ParameterConversionHandler<TContext> where TContext : IContext
{
    protected override Task<Unit> HandleAsync(ParameterConversionContext<TContext> context)
    {
        if (ParameterServiceProperties.Of(context.Parameter)?.ServiceType is not { } serviceType)
        {
            return Unit.Task;
        }

        var value = context.ServiceProvider.GetService(serviceType);

        if (value is null && !context.Parameter.IsOptional)
        {
            throw new ServiceNotFoundException(context.Parameter);
        }

        context.ValueResolved = true;
        context.ResolvedValue = value;

        return Unit.Task;
    }
}
