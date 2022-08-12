namespace Kantaiko.Controllers;

public interface IContextAcceptor<in TContext>
{
    void SetContext(TContext context);
}
