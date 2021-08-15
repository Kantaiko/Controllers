using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Kantaiko.Controllers.Matchers;

namespace Kantaiko.Controllers.Internal
{
    internal readonly struct ControllerMatchResult<TContext>
    {
        [MemberNotNullWhen(true, nameof(Controller), nameof(Endpoint), nameof(MatchResult))]
        public bool IsMatched => Controller is not null;

        public ControllerManager<TContext>? Controller { get; private init; }
        public EndpointManager<TContext>? Endpoint { get; private init; }
        public EndpointMatchResult? MatchResult { get; private init; }

        public static ControllerMatchResult<TContext> NotMatched => new();

        public static ControllerMatchResult<TContext> Matched(ControllerManager<TContext> controller,
            EndpointManager<TContext> endpoint,
            EndpointMatchResult matchResult) => new()
        {
            Controller = controller,
            Endpoint = endpoint,
            MatchResult = matchResult
        };

        public void Deconstruct(out ControllerManager<TContext> controller, out EndpointManager<TContext> endpoint,
            out EndpointMatchResult matchResult)
        {
            Debug.Assert(IsMatched);

            controller = Controller;
            endpoint = Endpoint;
            matchResult = MatchResult;
        }
    }
}
