namespace Kantaiko.Controllers.ParameterConversion;

/// <summary>
/// The delegate that converts a string value to a parameter value.
/// </summary>
public delegate ValueTask<ConversionResult> TextParameterDelegate(string value, ParameterConversionContext context);

/// <summary>
/// The generic version of the <see cref="TextParameterDelegate"/> delegate.
/// </summary>
/// <typeparam name="TValue">The type of the parameter value.</typeparam>
public delegate ValueTask<ConversionResult<TValue>> TextParameterDelegate<TValue>(string value,
    ParameterConversionContext context);
