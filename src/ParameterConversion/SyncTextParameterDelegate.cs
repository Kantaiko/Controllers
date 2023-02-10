namespace Kantaiko.Controllers.ParameterConversion;

/// <summary>
/// The synchronous delegate that converts a string value to a parameter value.
/// </summary>
/// <typeparam name="TValue">The type of the parameter value.</typeparam>
public delegate ConversionResult<TValue> SyncTextParameterDelegate<TValue>(string value,
    ParameterConversionContext context);
