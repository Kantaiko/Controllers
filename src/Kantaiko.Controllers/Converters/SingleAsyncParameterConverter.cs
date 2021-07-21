using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Validation;

namespace Kantaiko.Controllers.Converters
{
    public abstract class AsyncSingleParameterConverter<TParameter> : AsyncParameterConverter<TParameter>
    {
        public override Task<ResolutionResult<TParameter>> Resolve(ParameterConversionContext context,
            CancellationToken cancellationToken)
        {
            return Resolve(context, context.Parameters[context.Info.Name], cancellationToken);
        }

        public override ValidationResult Validate(ParameterConversionContext context)
        {
            return context.Parameters.TryGetValue(context.Info.Name, out var value)
                ? Validate(context, value)
                : ValidationResult.Success;
        }

        public override bool CheckValueExistence(ParameterConversionContext context)
        {
            return context.Parameters.ContainsKey(context.Info.Name);
        }

        public abstract Task<ResolutionResult<TParameter>> Resolve(ParameterConversionContext context,
            string value, CancellationToken cancellationToken);

        public virtual ValidationResult Validate(ParameterConversionContext context,
            string value) => ValidationResult.Success;
    }
}
