using Kantaiko.Controllers.Introspection.Factory.Context;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface IControllerFilter
{
    bool IsIncluded(ControllerFactoryContext context);
}
