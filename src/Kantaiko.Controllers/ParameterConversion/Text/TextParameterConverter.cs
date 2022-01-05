using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.ParameterConversion.Validation;

namespace Kantaiko.Controllers.ParameterConversion.Text;

public abstract class TextParameterConverter<TParameter> : ITextParameterConverter<TParameter>,
    IAutoRegistrableTextParameterConverter
{
    public abstract bool CheckValueExistence(TextParameterConversionContext context);

    public virtual ValidationResult Validate(TextParameterConversionContext context) => ValidationResult.Success;

    public abstract ResolutionResult<TParameter> Resolve(TextParameterConversionContext context);

    public virtual Task<ResolutionResult<TParameter>> ResolveAsync(TextParameterConversionContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Resolve(context));
    }
}
