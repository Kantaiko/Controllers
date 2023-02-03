using System.ComponentModel.DataAnnotations;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.ParameterValidation;

/// <summary>
/// The additional properties that may be attached to a ParameterValidationFailed error.
/// </summary>
public sealed class ValidationErrorProperties : PropertyClass<ValidationErrorProperties>
{
    /// <summary>
    /// The <see cref="ValidationAttribute"/> that caused the validation error.
    /// <br/>
    /// Will be null if the error was caused not by a <see cref="ValidationAttribute"/>.
    /// </summary>
    public ValidationAttribute? ValidationAttribute { get; init; }
}
