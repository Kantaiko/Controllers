using Kantaiko.Controllers.Internal;

namespace Kantaiko.Controllers
{
    public abstract class ControllerBase<TRequest> : IRequestAcceptor
    {
        protected TRequest Request { get; private set; } = default!;

        void IRequestAcceptor.SetRequest(object request)
        {
            Request = (TRequest) request;
        }
    }
}
