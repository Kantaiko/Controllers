using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.Converters.Basic
{
    public class LongParameter : SingleParameterConverter<long>
    {
        public override ResolutionResult<long> Convert(ParameterConversionContext context, string value)
        {
            return long.TryParse(value, out var result)
                ? ResolutionResult.Success(result)
                : ResolutionResult.Error(Locale.IntegerRequired);
        }
    }
}
