namespace Kantaiko.Controllers.ParameterConversion.Validation;

public readonly struct ValidationResult
{
    public ValidationResult(bool isValid, string? errorMessage = null)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    public bool IsValid { get; }

    public string? ErrorMessage { get; }

    public static ValidationResult Success { get; } = new(true);
    public static ValidationResult Error(string? message) => new(false, message);
}
