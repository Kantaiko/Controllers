using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.ParameterConversion.Text.Basic;

public class ShortConverter : SingleTextParameterConverter<short>
{
    protected override ResolutionResult<short> Resolve(TextParameterConversionContext context, string value)
    {
        return short.TryParse(value, out var result)
            ? ResolutionResult.Success(result)
            : ResolutionResult.Error(Locale.IntegerRequired);
    }
}
