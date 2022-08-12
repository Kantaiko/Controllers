using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.ParameterConversion.Text.Basic;

public class DoubleConverter : SingleTextParameterConverter<double>
{
    protected override ResolutionResult<double> Resolve(TextParameterConversionContext context, string value)
    {
        return double.TryParse(value, out var result)
            ? ResolutionResult.Success(result)
            : ResolutionResult.Error(Locale.NumberRequired);
    }
}
