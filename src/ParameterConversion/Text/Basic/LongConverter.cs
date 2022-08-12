using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.ParameterConversion.Text.Basic;

public class LongConverter : SingleTextParameterConverter<long>
{
    protected override ResolutionResult<long> Resolve(TextParameterConversionContext context, string value)
    {
        return long.TryParse(value, out var result)
            ? ResolutionResult.Success(result)
            : ResolutionResult.Error(Locale.IntegerRequired);
    }
}
