namespace Kantaiko.Controllers.Tests.Shared;

public class TestContext
{
    public TestContext(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
