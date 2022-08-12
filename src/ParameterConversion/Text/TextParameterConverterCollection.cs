using System;
using System.Collections.Generic;
using System.Linq;
using Kantaiko.Controllers.ParameterConversion.Text.Basic;
using Kantaiko.Controllers.ParameterConversion.Text.Exceptions;

namespace Kantaiko.Controllers.ParameterConversion.Text;

public class TextParameterConverterCollection : ITextParameterConverterCollection
{
    private readonly Dictionary<Type, Type> _converterTypes;

    public TextParameterConverterCollection(IEnumerable<Type>? lookupTypes = null)
    {
        _converterTypes = new Dictionary<Type, Type>
        {
            [typeof(bool)] = typeof(BoolConverter),
            [typeof(int)] = typeof(IntConverter),
            [typeof(long)] = typeof(LongConverter),
            [typeof(string)] = typeof(StringConverter),
            [typeof(short)] = typeof(ShortConverter),
            [typeof(float)] = typeof(FloatConverter),
            [typeof(double)] = typeof(DoubleConverter),
            [typeof(decimal)] = typeof(DecimalConverter)
        };

        if (lookupTypes is null)
        {
            return;
        }

        foreach (var lookupType in lookupTypes)
        {
            if (!lookupType.IsAssignableTo(typeof(IAutoRegistrableTextParameterConverter)))
            {
                continue;
            }

            var converterInterface = lookupType.GetInterfaces().FirstOrDefault(x =>
                x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ITextParameterConverter<>));

            if (converterInterface is null)
            {
                continue;
            }

            var parameterType = converterInterface.GetGenericArguments()[0];
            _converterTypes[parameterType] = lookupType;
        }
    }

    public bool HasConverter(Type parameterType)
    {
        return _converterTypes.ContainsKey(parameterType);
    }

    public Type ResolveConverterType(Type parameterType)
    {
        return _converterTypes.TryGetValue(parameterType, out var converterType)
            ? converterType
            : throw new ConverterNotFoundException(parameterType);
    }

    public static TextParameterConverterCollection Default { get; } = new();
}
