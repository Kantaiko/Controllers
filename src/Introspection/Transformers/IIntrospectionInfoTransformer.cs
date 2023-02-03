namespace Kantaiko.Controllers.Introspection.Transformers;

/// <summary>
/// Defines a contract for transforming introspection info.
/// </summary>
public interface IIntrospectionInfoTransformer
{
    /// <summary>
    /// Transforms the introspection.
    /// </summary>
    /// <param name="introspectionInfo">The introspection info to transform.</param>
    /// <param name="serviceProvider">The service provider to use.</param>
    /// <returns>The transformed introspection info or the same instance if no changes were made.</returns>
    IntrospectionInfo Transform(IntrospectionInfo introspectionInfo, IServiceProvider serviceProvider);
}
