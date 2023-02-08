using Kantaiko.Controllers.Execution;

namespace Kantaiko.Controllers.ParameterConversion;

/// <summary>
/// The extension methods for <see cref="ControllerExecutorBuilder" />.
/// </summary>
public static class ControllerExecutorBuilderExtensions
{
    private class ConfigurationProperties
    {
        public List<IParameterConverter> Converters { get; } = new();
        public Dictionary<Type, TextParameterDelegate> TextParameterConverters { get; } = new();
    }

    /// <summary>
    /// Adds the parameter conversion infrastructure to the controller executor.
    /// </summary>
    /// <param name="builder">The controller executor builder.</param>
    public static void AddParameterConversion(this ControllerExecutorBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var properties = builder.Properties.GetOrCreate<ConfigurationProperties>();

        builder.Transformers.Add(new ParameterConversionTransformer());
        builder.Handlers.Add(new ParameterConversionHandler(properties.Converters));
    }

    /// <summary>
    /// Adds the text parameter conversion infrastructure to the controller executor.
    /// <br/>
    /// Does the same as <see cref="AddParameterConversion" /> but also adds <see cref="TextParameterConverterAdapter"/>
    /// that supports text parameter converters.
    /// </summary>
    /// <param name="builder">The controller executor builder.</param>
    public static void AddTextParameterConversion(this ControllerExecutorBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var properties = builder.Properties.GetOrCreate<ConfigurationProperties>();
        properties.Converters.Add(new TextParameterConverterAdapter(properties.TextParameterConverters));

        builder.Transformers.Add(new ParameterConversionTransformer());
        builder.Handlers.Add(new ParameterConversionHandler(properties.Converters));
    }

    /// <summary>
    /// Adds a parameter converter to the controller executor.
    /// <br/>
    /// The <see cref="AddParameterConversion"/> method must be called.
    /// </summary>
    /// <param name="builder">The controller executor builder.</param>
    /// <param name="converter">The parameter converter to add.</param>
    public static void AddConverter(this ControllerExecutorBuilder builder, IParameterConverter converter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(converter);

        var properties = builder.Properties.GetOrCreate<ConfigurationProperties>();
        properties.Converters.Add(converter);
    }

    /// <summary>
    /// Adds a text parameter converter to the controller executor.
    /// If the converter for the specified type already exists, it will be replaced.
    /// <br/>
    /// The <see cref="AddTextParameterConversion"/> method must be called.
    /// </summary>
    /// <param name="builder">The controller executor builder.</param>
    /// <param name="converter">The text parameter converter to add.</param>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    public static void AddConverter<T>(this ControllerExecutorBuilder builder, ITextParameterConverter<T> converter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(converter);

        var properties = builder.Properties.GetOrCreate<ConfigurationProperties>();

        properties.TextParameterConverters[typeof(T)] = async (value, context) =>
        {
            var result = await converter.ConvertAsync(value, context);
            return new ConversionResult(result.Success, result.Value, result.Error);
        };
    }

    /// <summary>
    /// Adds a text parameter converter to the controller executor.
    /// If the converter for the specified type already exists, it will be replaced.
    /// <br/>
    /// The <see cref="AddTextParameterConversion"/> method must be called.
    /// </summary>
    /// <param name="builder">The controller executor builder.</param>
    /// <param name="converter">The text parameter converter to add.</param>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    public static void AddConverter<T>(this ControllerExecutorBuilder builder, TextParameterDelegate<T> converter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(converter);

        var properties = builder.Properties.GetOrCreate<ConfigurationProperties>();

        properties.TextParameterConverters[typeof(T)] = async (value, context) =>
        {
            var result = await converter(value, context);
            return new ConversionResult(result.Success, result.Value, result.Error);
        };
    }
}
