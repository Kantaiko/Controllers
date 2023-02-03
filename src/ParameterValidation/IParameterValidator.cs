using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.ParameterValidation;

/// <summary>
/// The interface for parameter validators.
/// </summary>
public interface IParameterValidator
{
    /// <summary>
    /// Validates the parameter value.
    /// </summary>
    /// <param name="context">The context of the parameter validation.</param>
    /// <returns>The result of the parameter validation.</returns>
    ValidationResult Validate(ParameterValidationContext context);
}

/// <summary>
/// The generic version of <see cref="IParameterValidator"/>.
/// </summary>
/// <typeparam name="TValue">The type of the value to validate.</typeparam>
public interface IParameterValidator<TValue> : IParameterValidator
{
    protected ValidationResult Validate(ParameterValidationContext<TValue> context);

    ValidationResult IParameterValidator.Validate(ParameterValidationContext context)
    {
        if (context.Value is null)
        {
            return Validate(new ParameterValidationContext<TValue>(default!,
                context.Parameter, context.ServiceProvider));
        }

        if (context.Value is not TValue value)
        {
            throw new InvalidOperationException(string.Format(Strings.InvalidValidatorValueType,
                context.Value.GetType(), typeof(TValue)));
        }

        return Validate(new ParameterValidationContext<TValue>(value, context.Parameter, context.ServiceProvider));
    }
}
