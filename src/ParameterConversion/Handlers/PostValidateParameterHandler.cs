using System.Threading.Tasks;
using Kantaiko.Controllers.ParameterConversion.Validation;
using Kantaiko.Controllers.Result;

namespace Kantaiko.Controllers.ParameterConversion.Handlers;

public class PostValidateParameterHandler<TContext> : IParameterConversionHandler<TContext>
{
    public Task HandleAsync(ParameterConversionContext<TContext> context)
    {
        if (PostValidationParameterProperties.Of(context.Parameter) is not { PostValidators: { } postValidators })
        {
            return Task.CompletedTask;
        }

        if (context.ResolvedValue is null)
        {
            return Task.CompletedTask;
        }

        var validationContext = new ParameterPostValidationContext(
            context.ResolvedValue,
            context.Parameter,
            context.ServiceProvider
        );

        foreach (var validator in postValidators)
        {
            var result = validator.Validate(validationContext);

            if (!result.IsValid)
            {
                context.ExecutionContext.ExecutionResult =
                    ControllerExecutionResult.Error(result.ErrorMessage, context.Parameter);

                return Task.CompletedTask;
            }
        }

        return Task.CompletedTask;
    }
}
