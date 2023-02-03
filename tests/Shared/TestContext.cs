namespace Kantaiko.Controllers.Tests.Shared;

internal class TestContext
{
    public TestContext(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
