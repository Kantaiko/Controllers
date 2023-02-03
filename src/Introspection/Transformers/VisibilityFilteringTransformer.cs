using System.Collections.Immutable;
using Kantaiko.Controllers.Introspection.Context;

namespace Kantaiko.Controllers.Introspection.Transformers;

/// <summary>
/// The transformer that filters out members that are not public or are inherited from a base class.
/// </summary>
public sealed class VisibilityFilteringTransformer : IntrospectionInfoTransformer
{
    private readonly bool _requirePublic;
    private readonly bool _allowInheritance;

    /// <summary>
    /// Creates a new instance of the <see cref="VisibilityFilteringTransformer"/> class.
    /// </summary>
    /// <param name="requirePublic">
    /// Whether to require public members.
    /// By default, only public members are included.
    /// </param>
    /// <param name="allowInheritance">
    /// Whether to allow members inherited from a base class.
    /// By default, both inherited and non-inherited members are included.
    /// </param>
    public VisibilityFilteringTransformer(bool requirePublic = true, bool allowInheritance = true)
    {
        _requirePublic = requirePublic;
        _allowInheritance = allowInheritance;
    }

    protected override IReadOnlyList<EndpointInfo> TransformEndpoints(ControllerTransformationContext context)
    {
        IEnumerable<EndpointInfo> endpoints = context.Controller.Endpoints;

        if (_requirePublic)
        {
            endpoints = endpoints.Where(x => x.MethodInfo.IsPublic);
        }

        if (!_allowInheritance)
        {
            endpoints = endpoints.Where(x => x.MethodInfo.GetBaseDefinition() == x.MethodInfo.DeclaringType);
        }

        return endpoints.ToImmutableArray();
    }
}
