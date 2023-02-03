using Kantaiko.Properties;

namespace Kantaiko.Controllers.ParameterConversion;

/// <summary>
/// The parameter properties that contain the list of parameter converters.
/// </summary>
/// <param name="Converters">The list of parameter converters.</param>
public record ParameterConversionProperties(IReadOnlyList<IParameterConverter> Converters) :
    PropertyRecord<ParameterConversionProperties>;
