using System.Diagnostics.CodeAnalysis;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Controllers.Matching;

public class EndpointMatchResult
{
    public bool IsMatched { get; private init; }

    [MemberNotNullWhen(true, nameof(ErrorMessage))]
    public bool HasError => ErrorMessage is not null;

    [MemberNotNullWhen(true, nameof(Properties))]
    public bool IsSuccess => IsMatched && !HasError;

    public IImmutablePropertyCollection? Properties { get; private init; }
    public string? ErrorMessage { get; private init; }

    public static EndpointMatchResult Success(IImmutablePropertyCollection properties) => new()
    {
        IsMatched = true,
        Properties = properties
    };

    public static EndpointMatchResult Error(string errorMessage) => new()
    {
        IsMatched = true,
        ErrorMessage = errorMessage
    };

    public static EndpointMatchResult NotMatched { get; } = new();
}
