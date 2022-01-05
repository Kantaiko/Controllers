using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.ParameterConversion.Validation;

namespace Kantaiko.Controllers.ParameterConversion.Text;

public interface ITextParameterConverter
{
    bool CheckValueExistence(TextParameterConversionContext context);

    ValidationResult Validate(TextParameterConversionContext context);

    Task<IResolutionResult> ResolveAsync(TextParameterConversionContext context,
        CancellationToken cancellationToken = default);
}

public interface ITextParameterConverter<TParameter> : ITextParameterConverter
{
    new Task<ResolutionResult<TParameter>> ResolveAsync(TextParameterConversionContext context,
        CancellationToken cancellationToken = default);

    async Task<IResolutionResult> ITextParameterConverter.ResolveAsync(TextParameterConversionContext context,
        CancellationToken cancellationToken)
    {
        return await ResolveAsync(context, cancellationToken);
    }
}
