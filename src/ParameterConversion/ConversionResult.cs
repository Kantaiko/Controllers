using Kantaiko.Controllers.Execution;

namespace Kantaiko.Controllers.ParameterConversion;

/// <summary>
/// The result of the parameter conversion.
/// </summary>
public readonly struct ConversionResult
{
    /// <summary>
    /// Creates a new instance of <see cref="ConversionResult"/>.
    /// </summary>
    /// <param name="success">Whether the conversion was successful.</param>
    /// <param name="value">The converted value.</param>
    /// <param name="error">The error that occurred during the conversion.</param>
    public ConversionResult(bool success, object? value = null, ControllerError? error = null)
    {
        Success = success;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Creates a new instance of <see cref="ConversionResult"/>.
    /// </summary>
    /// <param name="value">The converted value.</param>
    public ConversionResult(object? value)
    {
        Success = true;
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="ConversionResult"/>.
    /// </summary>
    /// <param name="error">The conversion error.</param>
    public ConversionResult(ControllerError? error)
    {
        Success = false;
        Error = error;
    }

    /// <summary>
    /// Whether the conversion was successful.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// The error that occurred during the conversion if any.
    /// </summary>
    public ControllerError? Error { get; }

    /// <summary>
    /// The converted value.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Creates a conversion result from the boolean value.
    /// </summary>
    /// <param name="success">Whether the conversion was successful.</param>
    /// <returns>The conversion result.</returns>
    public static implicit operator ConversionResult(bool success) => new(success);

    /// <summary>
    /// Creates a conversion result from the <see cref="ControllerError"/>.
    /// </summary>
    /// <param name="error">The conversion error.</param>
    /// <returns>The conversion result.</returns>
    public static implicit operator ConversionResult(ControllerError? error) => new(error);
}

/// <summary>
/// The generic version of <see cref="ConversionResult"/>.
/// </summary>
/// <typeparam name="TValue">The type of the converted value.</typeparam>
public readonly struct ConversionResult<TValue>
{
    /// <summary>
    /// Creates a new instance of <see cref="ConversionResult{TValue}"/>.
    /// </summary>
    /// <param name="success">Whether the conversion was successful.</param>
    public ConversionResult(bool success)
    {
        Success = success;
        Value = default!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="ConversionResult{TValue}"/>.
    /// </summary>
    /// <param name="value">The converted value.</param>
    public ConversionResult(TValue value)
    {
        Success = true;
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="ConversionResult{TValue}"/>.
    /// </summary>
    /// <param name="error">The conversion error.</param>
    public ConversionResult(ControllerError? error)
    {
        Success = false;
        Error = error;
        Value = default!;
    }

    /// <summary>
    /// Whether the conversion was successful.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// The error that occurred during the conversion if any.
    /// </summary>
    public ControllerError? Error { get; }

    /// <summary>
    /// The converted value.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// Creates a conversion result from the boolean value.
    /// </summary>
    /// <param name="success">Whether the conversion was successful.</param>
    /// <returns>The conversion result.</returns>
    public static implicit operator ConversionResult<TValue>(bool success) => new(success);

    /// <summary>
    /// Creates a conversion result from the <see cref="ControllerError"/>.
    /// </summary>
    /// <param name="error">The conversion error.</param>
    /// <returns>The conversion result.</returns>
    public static implicit operator ConversionResult<TValue>(ControllerError? error) => new(error);

    /// <summary>
    /// Creates a conversion result from the value.
    /// </summary>
    /// <param name="value">The converted value.</param>
    /// <returns>The conversion result.</returns>
    public static implicit operator ConversionResult<TValue>(TValue value) => new(value);
}
