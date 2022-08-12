namespace Kantaiko.Controllers.ParameterConversion.Validation;

public interface IParameterPostValidator
{
    ValidationResult Validate(ParameterPostValidationContext context);
}
