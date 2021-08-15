namespace Kantaiko.Controllers.Internal
{
    internal interface IContextAcceptor<TContext>
    {
        void SetContext(TContext request);
    }
}
