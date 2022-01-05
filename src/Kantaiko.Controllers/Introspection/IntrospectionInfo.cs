using System.Collections.Generic;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Introspection;

public record IntrospectionInfo : IImmutablePropertyContainer
{
    private readonly IReadOnlyList<ControllerInfo> _controllers;

    public IntrospectionInfo(IReadOnlyList<ControllerInfo> controllers, IImmutablePropertyCollection? properties = null)
    {
        _controllers = AddParentReferences(controllers);
        Properties = properties ?? ImmutablePropertyCollection.Empty;
    }

    public IReadOnlyList<ControllerInfo> Controllers
    {
        get => _controllers;
        init => _controllers = AddParentReferences(value);
    }

    public IImmutablePropertyCollection Properties { get; init; }

    private IReadOnlyList<ControllerInfo> AddParentReferences(IReadOnlyList<ControllerInfo> controllers)
    {
        foreach (var controller in controllers)
        {
            controller.IntrospectionInfo = this;
        }

        return controllers;
    }
}
