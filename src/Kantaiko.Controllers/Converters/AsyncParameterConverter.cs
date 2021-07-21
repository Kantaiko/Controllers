using Kantaiko.Controllers.Validation;

namespace Kantaiko.Controllers.Converters
{
    public abstract class AsyncParameterConverter<TParameter> : ParameterConverterBase<TParameter>
    {
        public override ValidationResult Validate(ParameterConversionContext context) =>
            ValidationResult.Success;
    }
}
