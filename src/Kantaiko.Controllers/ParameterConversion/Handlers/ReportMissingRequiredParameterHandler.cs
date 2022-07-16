using System.Threading.Tasks;
using Kantaiko.Controllers.Resources;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public class ReportMissingRequiredParameterHandler<TContext> : ParameterConversionHandler<TContext>
{
    protected override Task HandleAsync(ParameterConversionContext<TContext> context)
    {
        if (!context.ValueExists && !context.ValueResolved && !context.Parameter.IsOptional)
        {
            var errorMessage = string.Format(Locale.MissingRequiredParameter, context.Parameter.Name);

            context.ExecutionResult = ControllerResult.Error(errorMessage, context.Parameter);
        }

        return Task.CompletedTask;
    }
}
