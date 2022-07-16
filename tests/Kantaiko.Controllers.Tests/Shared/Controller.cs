namespace Kantaiko.Controllers.Tests.Shared;

internal abstract class Controller : IContextAcceptor<TestContext>, IAutoRegistrableController<TestContext>
{
    protected TestContext Context { get; private set; } = null!;

    void IContextAcceptor<TestContext>.SetContext(TestContext context)
    {
        Context = context;
    }
}
