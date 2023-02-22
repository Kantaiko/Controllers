using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Execution.Handlers;
using Kantaiko.Controllers.Resources;
using Kantaiko.Controllers.Utils;

namespace Kantaiko.Controllers.ParameterConversion;

/// <summary>
/// The execution handler that converts the parameters of the resolved endpoint using the matching result.
/// </summary>
public sealed class ParameterConversionHandler : IControllerExecutionHandler
{
    private readonly IEnumerable<IParameterConverter> _sharedConverters;

    /// <summary>
    /// Creates a new instance of <see cref="ParameterConversionHandler"/>.
    /// </summary>
    /// <param name="sharedConverters">The converters that used for all parameters after their own converters.</param>
    public ParameterConversionHandler(IEnumerable<IParameterConverter> sharedConverters)
    {
        _sharedConverters = sharedConverters;
    }

    async Task IControllerExecutionHandler.HandleAsync(ControllerExecutionContext context)
    {
        var endpoint = Check.ContextProperty(context.Endpoint, nameof(ParameterConversionHandler));

        var resolvedParameters = new object?[endpoint.Parameters.Count];

        for (var index = 0; index < endpoint.Parameters.Count; index++)
        {
            var parameter = endpoint.Parameters[index];
            var conversionContext = new ParameterConversionContext(parameter, context);

            if (ParameterConversionProperties.Of(parameter) is not { Converters: var converters })
            {
                throw new InvalidOperationException(Strings.NoParameterConversionProperties);
            }

            var converterSequence = converters.Concat(_sharedConverters);
            ConversionResult conversionResult = default;

            try
            {
                foreach (var converter in converterSequence)
                {
                    conversionResult = await converter.ConvertAsync(conversionContext);

                    if (conversionResult.Success) break;
                    if (conversionResult.Error is null) continue;

                    context.ExecutionError = new ControllerError(ControllerErrorCodes.ParameterConversionFailed)
                    {
                        Message = Strings.ParameterConversationFailed,
                        InnerError = conversionResult.Error,
                        Parameter = parameter,
                        Properties = context.MatchProperties
                    };

                    return;
                }
            }
            catch (Exception exception)
            {
                context.ExecutionError = new ControllerError(ControllerErrorCodes.ParameterConversionException)
                {
                    Message = Strings.ParameterConversionException,
                    Exception = exception,
                    Parameter = parameter,
                    Properties = context.MatchProperties
                };

                return;
            }

            if (!conversionResult.Success)
            {
                context.ExecutionError = new ControllerError(ControllerErrorCodes.NoSuitableParameterConverter)
                {
                    Message = string.Format(Strings.NoSuitableParameterConverter, parameter.ParameterType.FullName),
                    Parameter = parameter,
                    Properties = context.MatchProperties
                };

                return;
            }

            resolvedParameters[index] = conversionResult.Value;
        }

        context.ResolvedParameters = resolvedParameters;
    }
}
