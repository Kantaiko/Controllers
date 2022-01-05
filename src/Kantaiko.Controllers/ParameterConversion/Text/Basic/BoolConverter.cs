using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.ParameterConversion.Text.Basic;

public class BoolConverter : SingleTextParameterConverter<bool>
{
    protected override ResolutionResult<bool> Resolve(TextParameterConversionContext context, string value)
    {
        return bool.TryParse(value, out var result)
            ? ResolutionResult.Success(result)
            : ResolutionResult.Error(Locale.BoolRequired);
    }
}
