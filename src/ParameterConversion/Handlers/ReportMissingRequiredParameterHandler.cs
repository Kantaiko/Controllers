using System.Threading.Tasks;
using Kantaiko.Controllers.Resources;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public class ReportMissingRequiredParameterHandler<TContext> : IParameterConversionHandler<TContext>
{
    public Task HandleAsync(ParameterConversionContext<TContext> context)
    {
        if (!context.ValueExists && !context.ValueResolved && !context.Parameter.IsOptional)
        {
            var errorMessage = string.Format(Locale.MissingRequiredParameter, context.Parameter.Name);

            context.ExecutionContext.ExecutionResult = ControllerExecutionResult.Error(errorMessage, context.Parameter);
        }

        return Task.CompletedTask;
    }
}
