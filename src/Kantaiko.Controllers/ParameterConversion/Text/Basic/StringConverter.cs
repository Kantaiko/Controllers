namespace Kantaiko.Controllers.ParameterConversion.Text.Basic;

public class StringConverter : SingleTextParameterConverter<string>
{
    protected override ResolutionResult<string> Resolve(TextParameterConversionContext context, string value)
    {
        return ResolutionResult.Success(value);
    }
}
