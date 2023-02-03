using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.ParameterValidation;

namespace Kantaiko.Controllers.ParameterConversion;

/// <summary>
/// Defines the set of well-known error codes for parameter conversion and validation.
/// </summary>
public static class ParameterErrorCodes
{
    private const string Prefix = "WellKnown:";

    /// <summary>
    /// One of the required parameters was not provided.
    /// Should be handled and reported to the user.
    /// </summary>
    public const string MissingRequiredParameter = Prefix + nameof(MissingRequiredParameter);

    /// <summary>
    /// The provided text cannot be converted to an <see cref="int"/> value.
    /// </summary>
    public const string InvalidInt = Prefix + nameof(InvalidInt);

    /// <summary>
    /// The provided text cannot be converted to a <see cref="long"/> value.
    /// </summary>
    public const string InvalidLong = Prefix + nameof(InvalidLong);

    /// <summary>
    /// The provided text cannot be converted to a <see cref="double"/> value.
    /// </summary>
    public const string InvalidDouble = Prefix + nameof(InvalidDouble);

    /// <summary>
    /// The provided text cannot be converted to a <see cref="float"/> value.
    /// </summary>
    public const string InvalidFloat = Prefix + nameof(InvalidFloat);

    /// <summary>
    /// The provided text cannot be converted to a <see cref="decimal"/> value.
    /// </summary>
    public const string InvalidDecimal = Prefix + nameof(InvalidDecimal);

    /// <summary>
    /// The provided text cannot be converted to a <see cref="bool"/> value.
    /// </summary>
    public const string InvalidBool = Prefix + nameof(InvalidBool);

    /// <summary>
    /// The provided text cannot be converted to a <see cref="Guid"/> value.
    /// </summary>
    public const string InvalidGuid = Prefix + nameof(InvalidGuid);

    /// <summary>
    /// The provided text cannot be converted to a <see cref="DateTime"/> value.
    /// </summary>
    public const string InvalidDateTime = Prefix + nameof(InvalidDateTime);

    /// <summary>
    /// The provided text cannot be converted to a <see cref="DateTimeOffset"/> value.
    /// </summary>
    public const string InvalidDateTimeOffset = Prefix + nameof(InvalidDateTimeOffset);

    /// <summary>
    /// The provided text cannot be converted to a <see cref="TimeSpan"/> value.
    /// </summary>
    public const string InvalidTimeSpan = Prefix + nameof(InvalidTimeSpan);

    /// <summary>
    /// The ValidationAttribute failed to validate the parameter.
    /// <br/>
    /// The <see cref="ControllerError.Properties"/> will contain the <see cref="ValidationErrorProperties"/>
    /// with the <see cref="ValidationErrorProperties.ValidationAttribute"/> property set to the failed attribute.
    /// </summary>
    public const string ValidationAttributeFailed = Prefix + nameof(ValidationAttributeFailed);
}
