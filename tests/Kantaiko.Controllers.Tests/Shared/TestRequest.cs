using System.Collections.Generic;

namespace Kantaiko.Controllers.Tests.Shared
{
    public record TestContext(string Text,
        bool ShouldOverrideValueViaMiddleware = false,
        bool ShouldInterruptViaMiddleware = false,
        Dictionary<string, object>? Result = null);
}
