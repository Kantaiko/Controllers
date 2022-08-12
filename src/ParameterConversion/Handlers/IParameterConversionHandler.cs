using System.Threading.Tasks;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public interface IParameterConversionHandler<TContext>
{
    Task HandleAsync(ParameterConversionContext<TContext> context);
}
