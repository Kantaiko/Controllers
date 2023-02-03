namespace Kantaiko.Controllers.ParameterConversion;

/// <summary>
/// Defines a parameter converter that can transform endpoint matching result into a parameter value.
/// </summary>
public interface IParameterConverter
{
    /// <summary>
    /// Converts the endpoint matching result to a parameter value.
    /// </summary>
    /// <param name="context">The context of the parameter conversion.</param>
    /// <returns>
    /// A ValueTask that resolves to the ConversionResult containing the converted value or an error.
    /// </returns>
    ValueTask<ConversionResult> ConvertAsync(ParameterConversionContext context);
}
