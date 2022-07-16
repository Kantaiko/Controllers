using System.Threading.Tasks;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Controllers.Execution.Handlers;

public interface IControllerExecutionHandler<TContext> :
    IChainedHandler<ControllerContext<TContext>, Task<ControllerResult>> { }
