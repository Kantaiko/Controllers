using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Kantaiko.Controllers.Utils;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection;

public record EndpointInfo : IImmutablePropertyContainer
{
    private readonly IReadOnlyList<EndpointParameterInfo> _parameterTree;

    public EndpointInfo(MethodInfo methodInfo, IReadOnlyList<EndpointParameterInfo> parameterTree,
        IImmutablePropertyCollection? properties = null)
    {
        MethodInfo = methodInfo;

        _parameterTree = parameterTree;
        Parameters = AddParentReferences(Flatten(parameterTree).ToImmutableArray());

        Properties = properties ?? ImmutablePropertyCollection.Empty;
    }

    public IReadOnlyList<EndpointParameterInfo> ParameterTree
    {
        get => _parameterTree;
        init
        {
            _parameterTree = value;
            Parameters = AddParentReferences(Flatten(value).ToImmutableArray());
        }
    }

    public MethodInfo MethodInfo { get; init; }
    public IReadOnlyList<EndpointParameterInfo> Parameters { get; private init; }
    public IImmutablePropertyCollection Properties { get; init; }

    private static IEnumerable<EndpointParameterInfo> Flatten(IEnumerable<EndpointParameterInfo> parameters)
    {
        return parameters.SelectMany(parameterInfo =>
        {
            if (parameterInfo.HasChildren)
            {
                return Flatten(parameterInfo.Children);
            }

            return EnumerableUtils.Single(parameterInfo);
        });
    }

    private IReadOnlyList<EndpointParameterInfo> AddParentReferences(IReadOnlyList<EndpointParameterInfo> parameters)
    {
        foreach (var parameter in parameters)
        {
            parameter.Endpoint = this;
        }

        return parameters;
    }

    public ControllerInfo? Controller { get; internal set; }
}
