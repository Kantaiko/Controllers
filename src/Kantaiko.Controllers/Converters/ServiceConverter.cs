using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Validation;

namespace Kantaiko.Controllers.Converters
{
    internal class ServiceConverter : IParameterConverter
    {
        private object? _service;

        public bool CheckValueExistence(ParameterConversionContext context)
        {
            _service = context.ServiceProvider.GetService(context.Info.ParameterType);
            return _service is not null;
        }

        public ValidationResult Validate(ParameterConversionContext context) => ValidationResult.Success;

        public Task<IResolutionResult> Resolve(ParameterConversionContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<IResolutionResult>(ResolutionResult.Success(_service));
        }
    }
}
