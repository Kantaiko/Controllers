using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.ParameterConversion.Text.Basic;

public class FloatConverter : SingleTextParameterConverter<float>
{
    protected override ResolutionResult<float> Resolve(TextParameterConversionContext context, string value)
    {
        return float.TryParse(value, out var result)
            ? ResolutionResult.Success(result)
            : ResolutionResult.Error(Locale.NumberRequired);
    }
}
