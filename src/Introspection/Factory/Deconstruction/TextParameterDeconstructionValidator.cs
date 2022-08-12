using System;
using System.Linq;
using System.Reflection;
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

    public bool CanDeconstruct(Type type, ICustomAttributeProvider attributeProvider)
    {
        if (_converterCollection.HasConverter(type))
        {
            return false;
        }

        var hasConverterFactory = attributeProvider.GetCustomAttributes(true)
            .Any(x => x is ITextParameterConverterFactoryProvider or ITextParameterConverterTypeProvider);

        return !hasConverterFactory;
    }
}
