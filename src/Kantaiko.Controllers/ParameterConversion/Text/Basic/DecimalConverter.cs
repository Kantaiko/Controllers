using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.ParameterConversion.Text.Basic;

public class DecimalConverter : SingleTextParameterConverter<decimal>
{
    protected override ResolutionResult<decimal> Resolve(TextParameterConversionContext context, string value)
    {
        return decimal.TryParse(value, out var result)
            ? ResolutionResult.Success(result)
            : ResolutionResult.Error(Locale.NumberRequired);
    }
}
