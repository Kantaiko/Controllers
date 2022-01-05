using System;
using System.Linq;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.ParameterConversion.Text;

namespace Kantaiko.Controllers.Introspection.Factory.Deconstruction;

public class TextParameterDeconstructionValidator : IDeconstructionValidator
{
    private readonly ITextParameterConverterCollection _converterCollection;

    public TextParameterDeconstructionValidator(ITextParameterConverterCollection converterCollection)
    {
        _converterCollection = converterCollection;
    }

    public bool CanDeconstruct(Type type)
    {
        if (_converterCollection.HasConverter(type))
        {
            return false;
        }

        var hasConverterFactory = type.GetCustomAttributes(true)
            .Any(x => x is ITextParameterConverterFactoryProvider or ITextParameterConverterTypeProvider);

        return !hasConverterFactory;
    }
}
