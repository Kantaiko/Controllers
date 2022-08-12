using System.Threading.Tasks;

namespace Kantaiko.Controllers.Execution.Handlers;

public interface IControllerExecutionHandler<TContext>
{
    Task HandleAsync(ControllerExecutionContext<TContext> context);
}
