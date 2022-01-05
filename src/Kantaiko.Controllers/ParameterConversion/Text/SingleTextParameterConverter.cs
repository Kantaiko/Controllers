using Kantaiko.Controllers.ParameterConversion.Validation;

namespace Kantaiko.Controllers.ParameterConversion.Text;

public abstract class SingleTextParameterConverter<TParameter> : TextParameterConverter<TParameter>
{
    public override bool CheckValueExistence(TextParameterConversionContext context)
    {
        return context.Parameters.ContainsKey(context.ParameterInfo.Name);
    }

    protected abstract ResolutionResult<TParameter> Resolve(TextParameterConversionContext context, string value);

    public override ResolutionResult<TParameter> Resolve(TextParameterConversionContext context)
    {
        return Resolve(context, context.Parameters[context.ParameterInfo.Name]);
    }

    public override ValidationResult Validate(TextParameterConversionContext context)
    {
        return Validate(context, context.Parameters[context.ParameterInfo.Name]);
    }

    protected virtual ValidationResult Validate(TextParameterConversionContext context, string value) =>
        ValidationResult.Success;
}
