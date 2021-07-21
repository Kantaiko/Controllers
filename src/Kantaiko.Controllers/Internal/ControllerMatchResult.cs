using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Kantaiko.Controllers.Matchers;

namespace Kantaiko.Controllers.Internal
{
    internal readonly struct ControllerMatchResult<TRequest>
    {
        [MemberNotNullWhen(true, nameof(Controller), nameof(Endpoint), nameof(MatchResult))]
        public bool IsMatched => Controller is not null;

        public ControllerManager<TRequest>? Controller { get; private init; }
        public EndpointManager<TRequest>? Endpoint { get; private init; }
        public EndpointMatchResult? MatchResult { get; private init; }

        public static ControllerMatchResult<TRequest> NotMatched => new();

        public static ControllerMatchResult<TRequest> Matched(ControllerManager<TRequest> controller,
            EndpointManager<TRequest> endpoint,
            EndpointMatchResult matchResult) => new()
        {
            Controller = controller,
            Endpoint = endpoint,
            MatchResult = matchResult
        };

        public void Deconstruct(out ControllerManager<TRequest> controller, out EndpointManager<TRequest> endpoint,
            out EndpointMatchResult matchResult)
        {
            Debug.Assert(IsMatched);

            controller = Controller;
            endpoint = Endpoint;
            matchResult = MatchResult;
        }
    }
}
