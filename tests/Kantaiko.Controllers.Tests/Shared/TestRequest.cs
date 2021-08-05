using System.Collections.Generic;

namespace Kantaiko.Controllers.Tests.Shared
{
    public record TestRequest(string Text,
        bool ShouldOverrideValueViaMiddleware = false,
        bool ShouldInterruptViaMiddleware = false,
        Dictionary<string, object>? Result = null);
}
