using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Validation;

namespace Kantaiko.Controllers.Converters
{
    public interface IParameterConverter
    {
        bool CheckValueExistence(ParameterConversionContext context);
        ValidationResult Validate(ParameterConversionContext context);

        Task<IResolutionResult> Resolve(ParameterConversionContext context,
            CancellationToken cancellationToken);
    }
}
