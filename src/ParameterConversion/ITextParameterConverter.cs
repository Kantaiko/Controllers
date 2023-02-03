namespace Kantaiko.Controllers.ParameterConversion;

/// <summary>
/// The special parameter converter that uses the text value to convert the parameter.
/// </summary>
public interface ITextParameterConverter
{
    /// <summary>
    /// Converts the text value to a parameter value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="context">The context of the parameter conversion.</param>
    /// <returns>
    /// A ValueTask that resolves to the ConversionResult containing the converted value or an error.
    /// </returns>
    ValueTask<ConversionResult> ConvertAsync(string value, ParameterConversionContext context);
}

/// <summary>
/// The generic version of <see cref="ITextParameterConverter"/>.
/// </summary>
/// <typeparam name="TValue">The type of the parameter value.</typeparam>
public interface ITextParameterConverter<TValue> : ITextParameterConverter
{
    /// <summary>
    /// Converts the text value to a parameter value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="context">The context of the parameter conversion.</param>
    /// <returns>
    /// A ValueTask that resolves to the ConversionResult containing the converted value or an error.
    /// </returns>
    new ValueTask<ConversionResult<TValue>> ConvertAsync(string value, ParameterConversionContext context)
    {
        return ValueTask.FromResult(Convert(value, context));
    }

    /// <summary>
    /// Converts the text value to a parameter value.
    /// <br/>
    /// This is the synchronous version of <see cref="ConvertAsync"/>.
    /// It will be used in priority if the <see cref="ConvertAsync"/> method is not overridden.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="context">The context of the parameter conversion.</param>
    /// <exception cref="NotImplementedException">
    /// Neither <see cref="ConvertAsync"/> nor <see cref="Convert"/> are overridden.
    /// </exception>
    /// <returns>A ConversionResult containing the converted value or an error.</returns>
    ConversionResult<TValue> Convert(string value, ParameterConversionContext context)
    {
        throw new NotImplementedException("Either ConvertAsync or Convert must be overridden.");
    }

    async ValueTask<ConversionResult> ITextParameterConverter.ConvertAsync(string value,
        ParameterConversionContext context)
    {
        var result = await ConvertAsync(value, context);
        return new ConversionResult(result.Success, result.Value, result.Error);
    }
}
