using Kantaiko.Properties;

namespace Kantaiko.Controllers.ParameterConversion.Text;

public class TextParameterConversionProperties : PropertiesBase<TextParameterConversionProperties>
{
    public TextParameterConversionContext? ConversionContext { get; set; }
    public ITextParameterConverter? Converter { get; set; }
}
