using Kantaiko.Controllers.Middleware;

namespace Kantaiko.Controllers.Result
{
    public sealed class InterruptionExitReason : IExitReason
    {
        internal InterruptionExitReason(EndpointMiddlewareStage middlewareStage)
        {
            MiddlewareStage = middlewareStage;
        }

        public EndpointMiddlewareStage MiddlewareStage { get; }
    }
}
