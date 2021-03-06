using System.Diagnostics.CodeAnalysis;

namespace Kantaiko.Controllers.ParameterConversion;

public interface IResolutionResult
{
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public bool Success { get; }

    public string? ErrorMessage { get; }
    public object? Value { get; }
}
