using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Kantaiko.Controllers.Matchers
{
    public class EndpointMatchResult
    {
        public bool IsMatched { get; private init; }

        [MemberNotNullWhen(true, nameof(ErrorMessage))]
        public bool HasError => ErrorMessage is not null;

        [MemberNotNullWhen(true, nameof(Parameters))]
        public bool IsSuccess => IsMatched && !HasError;

        public IReadOnlyDictionary<string, string>? Parameters { get; private init; }
        public string? ErrorMessage { get; private init; }

        public static EndpointMatchResult Success(IReadOnlyDictionary<string, string> parameters) => new()
        {
            IsMatched = true,
            Parameters = parameters
        };

        public static EndpointMatchResult Error(string errorMessage) => new()
        {
            IsMatched = true,
            ErrorMessage = errorMessage
        };

        public static EndpointMatchResult NotMatched { get; } = new();
    }
}
