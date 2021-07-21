namespace Kantaiko.Controllers.Tests.Shared
{
    public record TestRequest(string Text,
        bool ShouldOverrideValueViaMiddleware = false,
        bool ShouldInterruptViaMiddleware = false);
}
