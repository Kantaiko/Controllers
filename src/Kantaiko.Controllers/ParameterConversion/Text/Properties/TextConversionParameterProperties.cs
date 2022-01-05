using System;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.ParameterConversion.Text.Properties;

public record TextConversionParameterProperties : ReadOnlyPropertiesBase<TextConversionParameterProperties>
{
    public Func<IServiceProvider, ITextParameterConverter>? ConverterFactory { get; init; }
    public Type? ConverterType { get; init; }
}
