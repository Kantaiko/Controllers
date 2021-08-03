using Kantaiko.Controllers.Internal;

namespace Kantaiko.Controllers
{
    public abstract class ControllerBase<TRequest> : IRequestAcceptor<TRequest>, IAutoRegistrableController
    {
        protected TRequest Request { get; private set; } = default!;

        void IRequestAcceptor<TRequest>.SetRequest(TRequest request)
        {
            Request = request;
        }
    }
}
