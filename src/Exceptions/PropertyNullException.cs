using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Kantaiko.Controllers.Exceptions;

public class PropertyNullException : Exception
{
    public PropertyNullException(string propertyName) :
        base($"Value cannot be null. (Property: {propertyName})") { }

    public static void ThrowIfNull([NotNull] object? value,
        [CallerArgumentExpression("value")] string? parameterName = null)
    {
        if (value is not null) return;

        Debug.Assert(parameterName is not null);
        throw new PropertyNullException(parameterName);
    }
}
