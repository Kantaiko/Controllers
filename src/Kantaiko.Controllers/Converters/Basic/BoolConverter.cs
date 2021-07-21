using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.Converters.Basic
{
    public class BoolConverter : SingleParameterConverter<bool>
    {
        public override ResolutionResult<bool> Convert(ParameterConversionContext context, string value)
        {
            return bool.TryParse(value, out var result)
                ? ResolutionResult.Success(result)
                : ResolutionResult.Error(Locale.BoolRequired);
        }
    }
}
