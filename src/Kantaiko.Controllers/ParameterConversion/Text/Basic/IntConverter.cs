using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.ParameterConversion.Text.Basic;

public class IntConverter : SingleTextParameterConverter<int>
{
    protected override ResolutionResult<int> Resolve(TextParameterConversionContext context, string value)
    {
        return int.TryParse(value, out var result)
            ? ResolutionResult.Success(result)
            : ResolutionResult.Error(Locale.IntegerRequired);
    }
}
