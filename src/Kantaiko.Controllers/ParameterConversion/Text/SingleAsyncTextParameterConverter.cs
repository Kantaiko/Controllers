using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.ParameterConversion.Validation;

namespace Kantaiko.Controllers.ParameterConversion.Text;

public abstract class SingleAsyncTextParameterConverter<TParameter> : AsyncTextParameterConverter<TParameter>
{
    public override bool CheckValueExistence(TextParameterConversionContext context)
    {
        return context.Parameters.ContainsKey(context.ParameterInfo.Name);
    }

    protected abstract Task<ResolutionResult<TParameter>> ResolveAsync(TextParameterConversionContext context,
        string value, CancellationToken cancellationToken = default);

    public override Task<ResolutionResult<TParameter>> ResolveAsync(TextParameterConversionContext context,
        CancellationToken cancellationToken = default)
    {
        return ResolveAsync(context, context.Parameters[context.ParameterInfo.Name], cancellationToken);
    }

    public override ValidationResult Validate(TextParameterConversionContext context)
    {
        return Validate(context, context.Parameters[context.ParameterInfo.Name]);
    }

    protected virtual ValidationResult Validate(TextParameterConversionContext context, string value) =>
        ValidationResult.Success;
}
