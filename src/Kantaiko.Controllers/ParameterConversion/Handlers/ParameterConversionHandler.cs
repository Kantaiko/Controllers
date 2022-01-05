using System.Threading.Tasks;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public abstract class ParameterConversionHandler<TContext> :
    IHandler<ParameterConversionContext<TContext>, Task<Unit>>
    where TContext : IContext
{
    protected abstract Task<Unit> HandleAsync(ParameterConversionContext<TContext> context);

    public Task<Unit> Handle(ParameterConversionContext<TContext> input)
    {
        return HandleAsync(input);
    }
}
