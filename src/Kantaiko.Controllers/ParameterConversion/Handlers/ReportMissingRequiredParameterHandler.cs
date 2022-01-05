using System.Threading.Tasks;
using Kantaiko.Controllers.Resources;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public class ReportMissingRequiredParameterHandler<TContext> : ParameterConversionHandler<TContext>
    where TContext : IContext
{
    protected override Task<Unit> HandleAsync(ParameterConversionContext<TContext> context)
    {
        if (!context.ValueExists && !context.ValueResolved && !context.Parameter.IsOptional)
        {
            var errorMessage = string.Format(Locale.MissingRequiredParameter, context.Parameter.Name);
            context.ExecutionResult = ControllerExecutionResult.Error(errorMessage, context.Parameter);
        }

        return Unit.Task;
    }
}
