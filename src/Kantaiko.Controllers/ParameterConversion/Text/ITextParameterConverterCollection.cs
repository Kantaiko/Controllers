using System;

namespace Kantaiko.Controllers.ParameterConversion.Text;

public interface ITextParameterConverterCollection
{
    bool HasConverter(Type parameterType);
    Type ResolveConverterType(Type parameterType);
}
