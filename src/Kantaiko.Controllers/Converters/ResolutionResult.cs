using System.Diagnostics.CodeAnalysis;

namespace Kantaiko.Controllers.Converters
{
    public readonly struct ResolutionResult<T> : IResolutionResult
    {
        internal ResolutionResult(bool success, string? errorMessage = default, T? value = default)
        {
            Success = success;
            ErrorMessage = errorMessage;
            Value = value;
        }

        [MemberNotNullWhen(false, nameof(ErrorMessage))]
        public bool Success { get; }

        public string? ErrorMessage { get; }
        public T? Value { get; }

        object? IResolutionResult.Value => Value;

        public static implicit operator ResolutionResult<T>(EmptyConversionResult conversionResult) =>
            new(conversionResult.Success, conversionResult.ErrorMessage);
    }

    public class EmptyConversionResult
    {
        internal EmptyConversionResult(bool success, string? errorMessage = default)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        internal bool Success { get; }
        internal string? ErrorMessage { get; }
    }

    public static class ResolutionResult
    {
        public static ResolutionResult<T> Success<T>(T value) => new(true, value: value);
        public static EmptyConversionResult Error(string message) => new(false, message);
    }
}
