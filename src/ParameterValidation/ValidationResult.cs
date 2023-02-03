using Kantaiko.Controllers.Execution;

namespace Kantaiko.Controllers.ParameterValidation;

/// <summary>
/// The result of the parameter validation.
/// </summary>
public readonly struct ValidationResult
{
    /// <summary>
    /// Creates a new instance of <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="success">Whether the validation was successful.</param>
    /// <param name="error">The validation error.</param>
    public ValidationResult(bool success, ControllerError? error)
    {
        Success = success;
        Error = error;
    }

    /// <summary>
    /// Whether the validation was successful.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// The validation error.
    /// </summary>
    public ControllerError? Error { get; }

    /// <summary>
    /// Creates a validation result from the boolean value.
    /// </summary>
    /// <param name="success">Whether the validation was successful.</param>
    /// <returns>The validation result.</returns>
    public static implicit operator ValidationResult(bool success) => new(success, null);

    /// <summary>
    /// Creates a validation result from the <see cref="ControllerError"/>.
    /// </summary>
    /// <param name="error">The validation error.</param>
    /// <returns>The validation result.</returns>
    public static implicit operator ValidationResult(ControllerError error) => new(false, error);
}
