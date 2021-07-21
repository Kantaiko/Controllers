using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.Converters.Basic
{
    public class IntConverter : SingleParameterConverter<int>
    {
        public override ResolutionResult<int> Convert(ParameterConversionContext context, string value)
        {
            return int.TryParse(value, out var result)
                ? ResolutionResult.Success(result)
                : ResolutionResult.Error(Locale.IntegerRequired);
        }
    }
}
