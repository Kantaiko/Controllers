using System.Linq;
using Kantaiko.Controllers.Introspection.Factory.Attributes;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.ParameterConversion.Text;
using Kantaiko.Controllers.ParameterConversion.Text.Properties;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection.Factory.Transformers;

public class TextParameterConversionIntrospectionInfoTransformer : IntrospectionInfoTransformer
{
    private readonly ITextParameterConverterCollection _converterCollection;

    public TextParameterConversionIntrospectionInfoTransformer(ITextParameterConverterCollection converterCollection)
    {
        _converterCollection = converterCollection;
    }

    protected override IImmutablePropertyCollection TransformParameterProperties(ParameterFactoryContext context)
    {
        if (context.Parameter.HasChildren)
        {
            return context.Parameter.Properties;
        }

        var attributes = context.Parameter.AttributeProvider.GetCustomAttributes(true);

        var typeProvider = attributes.OfType<ITextParameterConverterTypeProvider>().FirstOrDefault();

        if (typeProvider is not null)
        {
            return context.Parameter.Properties.Set(new TextConversionParameterProperties
            {
                ConverterType = typeProvider.GetTextParameterConverterType(context)
            });
        }

        var factoryProvider = attributes.OfType<ITextParameterConverterFactoryProvider>().FirstOrDefault();

        if (factoryProvider is not null)
        {
            return context.Parameter.Properties.Set(new TextConversionParameterProperties
            {
                ConverterFactory = factoryProvider.GetTextParameterConverterFactory(context)
            });
        }

        return context.Parameter.Properties.Set(new TextConversionParameterProperties
        {
            ConverterType = _converterCollection.ResolveConverterType(context.Parameter.ParameterType)
        });
    }
}
