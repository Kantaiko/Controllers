using System.ComponentModel.DataAnnotations;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.ParameterValidation;

/// <summary>
/// The properties of the parameter containing the list of validators and validation attributes.
/// </summary>
/// <param name="Validators">The list of validators.</param>
/// <param name="ValidationAttributes">The list of validation attributes.</param>
public sealed record ParameterValidationProperties(
    IReadOnlyList<IParameterValidator> Validators,
    IReadOnlyList<ValidationAttribute> ValidationAttributes
) : PropertyRecord<ParameterValidationProperties>;
