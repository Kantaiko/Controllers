namespace Kantaiko.Controllers.Internal
{
    internal interface IRequestAcceptor<TRequest>
    {
        void SetRequest(TRequest request);
    }
}
