namespace Kantaiko.Controllers.Converters.Basic
{
    public class StringConverter : SingleParameterConverter<string>
    {
        public override ResolutionResult<string> Convert(ParameterConversionContext context, string value)
        {
            return ResolutionResult.Success(value);
        }
    }
}
