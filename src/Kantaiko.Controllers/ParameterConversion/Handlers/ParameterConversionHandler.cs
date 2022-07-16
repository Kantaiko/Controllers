using System.Threading.Tasks;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public abstract class ParameterConversionHandler<TContext> : IParameterConversionHandler<TContext>
{
    protected abstract Task HandleAsync(ParameterConversionContext<TContext> context);

    public Task Handle(ParameterConversionContext<TContext> input)
    {
        return HandleAsync(input);
    }
}
