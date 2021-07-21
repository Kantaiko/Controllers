using System.Diagnostics.CodeAnalysis;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Internal
{
    internal readonly struct OperationResult
    {
        [MemberNotNullWhen(false, nameof(ErrorMessage))]
        public bool IsSuccess => ErrorMessage is null;

        public string? ErrorMessage { get; private init; }
        public EndpointParameterInfo? Parameter { get; private init; }

        public static OperationResult Success => new();

        public static OperationResult Failure(string errorMessage, EndpointParameterInfo? parameter = null) =>
            new() {ErrorMessage = errorMessage, Parameter = parameter};
    }
}
