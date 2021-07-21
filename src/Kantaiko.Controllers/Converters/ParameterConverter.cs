using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Validation;

namespace Kantaiko.Controllers.Converters
{
    public abstract class ParameterConverter<TParameter> : ParameterConverterBase<TParameter>
    {
        private ResolutionResult<TParameter>? _result;

        public override ValidationResult Validate(ParameterConversionContext context)
        {
            var resolutionResult = Convert(context);
            if (!resolutionResult.Success)
                return ValidationResult.Error(resolutionResult.ErrorMessage);

            _result = resolutionResult;
            return ValidationResult.Success;
        }

        public override Task<ResolutionResult<TParameter>> Resolve(ParameterConversionContext context,
            CancellationToken cancellationToken)
        {
            Debug.Assert(_result.HasValue);
            return Task.FromResult(_result.Value);
        }

        public abstract ResolutionResult<TParameter> Convert(ParameterConversionContext context);
    }
}
