using System.Reflection;
using Kantaiko.Controllers.Execution;

namespace Kantaiko.Controllers.Tests.Shared;

internal static class TestUtils
{
    public static ControllerExecutor CreateControllerExecutor<T>(Action<ControllerExecutorBuilder> configure)
    {
        var lookupTypes = typeof(T).GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);

        var builder = ControllerExecutorBuilder.CreateDefault();
        configure(builder);

        return builder.Build<Controller>(lookupTypes);
    }
}
