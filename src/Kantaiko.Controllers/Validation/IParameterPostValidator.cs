namespace Kantaiko.Controllers.Validation
{
    public interface IParameterPostValidator
    {
        ValidationResult Validate(ParameterPostValidationContext context, object value);
    }
}
