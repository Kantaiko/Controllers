using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Validation;

namespace Kantaiko.Controllers.Converters
{
    public abstract class ParameterConverterBase<TParameter> :
        IParameterConverter,
        IAutoRegistrableConverter<TParameter>
    {
        public abstract bool CheckValueExistence(ParameterConversionContext context);
        public abstract ValidationResult Validate(ParameterConversionContext context);

        async Task<IResolutionResult> IParameterConverter.Resolve(ParameterConversionContext context,
            CancellationToken cancellationToken) => await Resolve(context, cancellationToken);

        public abstract Task<ResolutionResult<TParameter>> Resolve(
            ParameterConversionContext context,
            CancellationToken cancellationToken);
    }
}
