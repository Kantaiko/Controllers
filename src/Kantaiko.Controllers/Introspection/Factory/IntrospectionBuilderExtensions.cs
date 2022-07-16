using Kantaiko.Controllers.Introspection.Factory.Deconstruction;
using Kantaiko.Controllers.Introspection.Factory.Transformers;
using Kantaiko.Controllers.ParameterConversion.Text;

namespace Kantaiko.Controllers.Introspection.Factory;

public static class IntrospectionBuilderExtensions
{
    public static void AddFilteringAttributes<TContext>(this IntrospectionBuilder<TContext> builder,
        bool requireControllerFilter = false, bool requireEndpointFilter = false)
    {
        builder.AddTransformer(new AttributeFilteringIntrospectionInfoTransformer(
            requireControllerFilter, requireEndpointFilter));
    }

    public static void AddVisibilityFiltering<TContext>(this IntrospectionBuilder<TContext> builder,
        bool requirePublic = true, bool allowInheritance = true)
    {
        builder.AddTransformer(new VisibilityFilteringIntrospectionInfoTransformer(requirePublic, allowInheritance));
    }

    public static void AddPropertyProviderAttributes<TContext>(this IntrospectionBuilder<TContext> builder)
    {
        builder.AddTransformer(new PropertyProviderIntrospectionInfoTransformer());
    }

    public static void AddParameterCustomization<TContext>(this IntrospectionBuilder<TContext> builder)
    {
        builder.AddTransformer(new CustomizationIntrospectionInfoTransformer());
    }

    public static void AddEndpointMatching<TContext>(this IntrospectionBuilder<TContext> builder)
    {
        builder.AddTransformer(new MatchingIntrospectionInfoTransformer<TContext>());
    }

    public static void AddParameterPostValidation<TContext>(this IntrospectionBuilder<TContext> builder)
    {
        builder.AddTransformer(new PostValidationIntrospectionInfoTransformer());
    }

    public static void AddSubHandlerAttributes<TContext>(this IntrospectionBuilder<TContext> builder)
    {
        builder.AddTransformer(new ExecutionIntrospectionInfoTransformer<TContext>());
    }

    public static void AddParameterDefaultValueResolution<TContext>(this IntrospectionBuilder<TContext> builder)
    {
        builder.AddTransformer(new DefaultValueIntrospectionInfoTransformer());
    }

    public static void AddDefaultTransformation<TContext>(this IntrospectionBuilder<TContext> builder)
    {
        builder.AddVisibilityFiltering();
        builder.AddFilteringAttributes();
        builder.AddPropertyProviderAttributes();
        builder.AddSubHandlerAttributes();
        builder.AddParameterCustomization();
        builder.AddParameterPostValidation();
        builder.AddParameterDefaultValueResolution();
    }

    public static void AddTextParameterConversion<TContext>(this IntrospectionBuilder<TContext> builder,
        ITextParameterConverterCollection? converterCollection = null)
    {
        converterCollection ??= TextParameterConverterCollection.Default;

        builder.AddTransformer(new TextParameterConversionIntrospectionInfoTransformer(converterCollection));
        builder.SetDeconstructionValidator(new TextParameterDeconstructionValidator(converterCollection));
    }
}
