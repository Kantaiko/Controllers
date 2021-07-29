using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Exceptions;
using Kantaiko.Controllers.Validation;

namespace Kantaiko.Controllers.Converters
{
    internal class ServiceConverter : IParameterConverter
    {
        public bool CheckValueExistence(ParameterConversionContext context) => true;

        public ValidationResult Validate(ParameterConversionContext context) => ValidationResult.Success;

        public Task<IResolutionResult> Resolve(ParameterConversionContext context, CancellationToken cancellationToken)
        {
            var service = context.ServiceProvider.GetService(context.Info.ParameterType);

            if (service is null && !context.Info.IsOptional)
                throw new ServiceNotFoundException(context.Info);

            return Task.FromResult<IResolutionResult>(ResolutionResult.Success(service));
        }
    }
}
