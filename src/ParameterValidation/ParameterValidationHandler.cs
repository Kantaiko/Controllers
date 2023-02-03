using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.Resources;
using Kantaiko.Controllers.Utils;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.ParameterValidation;

/// <summary>
/// The handler that validates the resolved parameter values.
/// </summary>
public sealed class ParameterValidationHandler : IControllerExecutionHandler
{
    Task IControllerExecutionHandler.HandleAsync(ControllerExecutionContext context)
    {
        var endpoint = Check.ContextProperty(context.Endpoint, nameof(ParameterValidationHandler));
        var resolvedParameters = Check.ResolvedParameters(context);

        for (var index = 0; index < endpoint.Parameters.Count; index++)
        {
            var parameter = endpoint.Parameters[index];

            if (ParameterValidationProperties.Of(parameter) is not
                { Validators: var validators, ValidationAttributes: var validationAttributes })
            {
                continue;
            }

            var validationContext = new ParameterValidationContext(resolvedParameters[index],
                parameter, context.ServiceProvider);

            foreach (var validator in validators)
            {
                ValidationResult validationResult;

                try
                {
                    validationResult = validator.Validate(validationContext);
                }
                catch (Exception exception)
                {
                    context.ExecutionError = new ControllerError(ControllerErrorCodes.ParameterValidationException)
                    {
                        Message = Strings.ParameterValidationException,
                        Exception = exception,
                        Parameter = parameter,
                    };

                    return Task.CompletedTask;
                }

                if (validationResult.Error is null) continue;

                context.ExecutionError = new ControllerError(ControllerErrorCodes.ParameterValidationFailed)
                {
                    Message = Strings.ParameterValidationFailed,
                    InnerError = validationResult.Error,
                    Parameter = parameter,
                };

                return Task.CompletedTask;
            }

            foreach (var validationAttribute in validationAttributes)
            {
                try
                {
                    var isValid = validationAttribute.IsValid(resolvedParameters[index]);
                    if (isValid) continue;

                    context.ExecutionError = new ControllerError(ControllerErrorCodes.ParameterValidationFailed)
                    {
                        Message = Strings.ParameterValidationFailed,
                        Parameter = parameter,
                        InnerError = new ControllerError(ParameterErrorCodes.ValidationAttributeFailed)
                        {
                            Message = validationAttribute.FormatErrorMessage(parameter.Name)
                        },
                        Properties = ImmutablePropertyCollection.Empty.Set(new ValidationErrorProperties
                        {
                            ValidationAttribute = validationAttribute,
                        })
                    };

                    return Task.CompletedTask;
                }
                catch (Exception exception)
                {
                    context.ExecutionError = new ControllerError(ControllerErrorCodes.ParameterValidationException)
                    {
                        Message = Strings.ParameterValidationException,
                        Exception = exception,
                        Parameter = parameter,
                    };

                    return Task.CompletedTask;
                }
            }
        }

        return Task.CompletedTask;
    }
}
