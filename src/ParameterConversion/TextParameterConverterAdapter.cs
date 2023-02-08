using Kantaiko.Controllers.EndpointMatching.Text;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Resources;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.ParameterConversion;

/// <summary>
/// The special parameter converter that looks up the text parameter converter and uses it to convert the parameter.
/// </summary>
public sealed class TextParameterConverterAdapter : IParameterConverter
{
    private readonly IReadOnlyDictionary<Type, TextParameterDelegate> _converters;

    /// <summary>
    /// Creates a new instance of <see cref="TextParameterConverterAdapter"/>.
    /// </summary>
    /// <param name="converters">The dictionary of text parameter converters.</param>
    public TextParameterConverterAdapter(IReadOnlyDictionary<Type, TextParameterDelegate> converters)
    {
        _converters = converters;
    }

    async ValueTask<ConversionResult> IParameterConverter.ConvertAsync(ParameterConversionContext context)
    {
        var matchProperties = context.MatchProperties.Get<TextMatchProperties>();
        var parameter = context.Parameter;

        if (matchProperties is null)
        {
            throw new InvalidOperationException(Strings.NoTextPatternMatchProperties);
        }

        if (!matchProperties.Parameters.TryGetValue(parameter.Name, out var value))
        {
            if (parameter.IsOptional)
            {
                return new ConversionResult(true);
            }

            return new ControllerError(ParameterErrorCodes.MissingRequiredParameter)
            {
                Message = string.Format(Strings.MissingRequiredParameter, context.Parameter.Name),
                Parameter = context.Parameter
            };
        }

        if (_converters.TryGetValue(parameter.ParameterType, out var customConverter))
        {
            return await customConverter.Invoke(value, context);
        }

        if (CommonTypeConverters.TryGetValue(parameter.ParameterType, out var commonTypeConverter))
        {
            return commonTypeConverter(value);
        }

        return false;
    }

    private static readonly Dictionary<Type, Func<string, ConversionResult>> CommonTypeConverters = new()
    {
        [typeof(string)] = value => new ConversionResult(value),

        [typeof(int)] = value => int.TryParse(value, out var result)
            ? new ConversionResult(result)
            : new ControllerError(ParameterErrorCodes.InvalidInt),

        [typeof(long)] = value => long.TryParse(value, out var result)
            ? new ConversionResult(result)
            : new ControllerError(ParameterErrorCodes.InvalidLong),

        [typeof(float)] = value => float.TryParse(value, out var result)
            ? new ConversionResult(result)
            : new ControllerError(ParameterErrorCodes.InvalidFloat),

        [typeof(double)] = value => double.TryParse(value, out var result)
            ? new ConversionResult(result)
            : new ControllerError(ParameterErrorCodes.InvalidDouble),

        [typeof(decimal)] = value => decimal.TryParse(value, out var result)
            ? new ConversionResult(result)
            : new ControllerError(ParameterErrorCodes.InvalidDecimal),

        [typeof(bool)] = value => bool.TryParse(value, out var result)
            ? new ConversionResult(result)
            : new ControllerError(ParameterErrorCodes.InvalidBool),

        [typeof(Guid)] = value => Guid.TryParse(value, out var result)
            ? new ConversionResult(result)
            : new ControllerError(ParameterErrorCodes.InvalidGuid),

        [typeof(DateTime)] = value => DateTime.TryParse(value, out var result)
            ? new ConversionResult(result)
            : new ControllerError(ParameterErrorCodes.InvalidDateTime),

        [typeof(DateTimeOffset)] = value => DateTimeOffset.TryParse(value, out var result)
            ? new ConversionResult(result)
            : new ControllerError(ParameterErrorCodes.InvalidDateTimeOffset),

        [typeof(TimeSpan)] = value => TimeSpan.TryParse(value, out var result)
            ? new ConversionResult(result)
            : new ControllerError(ParameterErrorCodes.InvalidTimeSpan),
    };
}
