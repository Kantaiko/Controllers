namespace Kantaiko.Controllers.Execution;

/// <summary>
/// Defines the set of well-known controller error codes.
/// <br/>
/// Since the error codes are identified by their string values, there are can be user-defined error codes
/// which are not listed here. Mind it when checking the error code.
/// </summary>
public static class ControllerErrorCodes
{
    private const string Prefix = "WellKnown:";

    /// <summary>
    /// No endpoint was found for the request.
    /// In most cases it's not an actual error, but it should be handled by the client.
    /// </summary>
    public const string NotMatched = Prefix + nameof(NotMatched);

    /// <summary>
    /// An error was returned by one of the endpoint matchers.
    /// In most cases should be handled by the client.
    /// </summary>
    public const string MatchingFailed = Prefix + nameof(MatchingFailed);

    /// <summary>
    /// An exception was thrown by one of the endpoint matchers.
    /// Should rather be fixed than handled.
    /// </summary>
    public const string MatchingException = Prefix + nameof(MatchingException);

    /// <summary>
    /// An exception was thrown by the endpoint itself.
    /// </summary>
    public const string InvocationException = Prefix + nameof(InvocationException);

    /// <summary>
    /// An exception was thrown by one of the execution pipeline handlers.
    /// Should rather be fixed than handled.
    /// </summary>
    public const string PipelineException = Prefix + nameof(PipelineException);

    /// <summary>
    /// An exception was thrown by a parameter converter.
    /// Should rather be fixed than handled.
    /// </summary>
    public const string ParameterConversionException = Prefix + nameof(ParameterConversionException);

    /// <summary>
    /// One of the parameters is invalid.
    /// The InnerError will contain the error returned by the parameter converter.
    /// Should be handled and reported to the user.
    /// </summary>
    public const string ParameterConversionFailed = Prefix + nameof(ParameterConversionFailed);

    /// <summary>
    /// There are no parameter converters registered for the parameter type.
    /// Should rather be fixed than handled.
    /// </summary>
    public const string NoSuitableParameterConverter = Prefix + nameof(NoSuitableParameterConverter);

    /// <summary>
    /// An exception was thrown by a parameter validator.
    /// Should rather be fixed than handled.
    /// </summary>
    public const string ParameterValidationException = Prefix + nameof(ParameterValidationException);

    /// <summary>
    /// One of the parameters is invalid.
    /// The InnerError will contain the error returned by the parameter validator.
    /// Should be handled and reported to the user.
    /// </summary>
    public const string ParameterValidationFailed = Prefix + nameof(ParameterValidationFailed);
}
