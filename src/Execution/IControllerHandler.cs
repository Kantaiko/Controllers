using System;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.Execution;

public interface IControllerHandler<in TContext>
{
    Task<ControllerExecutionResult> HandleAsync(
        TContext context,
        IServiceProvider? serviceProvider = null,
        CancellationToken cancellationToken = default
    );
}
