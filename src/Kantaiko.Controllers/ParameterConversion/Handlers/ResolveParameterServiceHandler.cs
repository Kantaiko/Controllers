using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.ParameterConversion.Properties;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public class ResolveServiceParameterHandler<TContext> : IParameterConversionHandler<TContext>
{
    public Task HandleAsync(ParameterConversionContext<TContext> context)
    {
        if (ParameterServiceProperties.Of(context.Parameter)?.ServiceType is not { } serviceType)
        {
            return Task.CompletedTask;
        }

        var value = context.ServiceProvider.GetService(serviceType);

        if (value is null && !context.Parameter.IsOptional)
        {
            throw new ServiceNotFoundException(context.Parameter);
        }

        context.ValueResolved = true;
        context.ResolvedValue = value;

        return Task.CompletedTask;
    }
}
