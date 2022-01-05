using System.Threading.Tasks;
using Kantaiko.Controllers.ParameterConversion.Validation;
using Kantaiko.Controllers.Result;
using Kantaiko.Routing;
using Kantaiko.Routing.Context;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public class PostValidateParameterHandler<TContext> : ParameterConversionHandler<TContext> where TContext : IContext
{
    protected override Task<Unit> HandleAsync(ParameterConversionContext<TContext> context)
    {
        if (PostValidationParameterProperties.Of(context.Parameter)?.PostValidators is not { } postValidators)
        {
            return Unit.Task;
        }

        if (context.ResolvedValue is null)
        {
            return Unit.Task;
        }

        var validationContext = new ParameterPostValidationContext(context.ResolvedValue,
            context.Parameter, context.ServiceProvider);

        foreach (var validator in postValidators)
        {
            var result = validator.Validate(validationContext);

            if (!result.IsValid)
            {
                context.ExecutionResult = ControllerExecutionResult.Error(result.ErrorMessage);
                return Unit.Task;
            }
        }

        return Unit.Task;
    }
}
