using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Result
{
    public sealed class ErrorExitReason : IExitReason
    {
        internal ErrorExitReason(RequestErrorStage stage, string errorMessage, EndpointParameterInfo? parameter)
        {
            Stage = stage;
            ErrorMessage = errorMessage;
            Parameter = parameter;
        }

        public RequestErrorStage Stage { get; }
        public string ErrorMessage { get; }
        public EndpointParameterInfo? Parameter { get; }
    }
}
