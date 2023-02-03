using System.Reflection;
using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.Exceptions;

/// <summary>
/// The exception that is thrown when an endpoint contains two composite parameters with the same type.
/// </summary>
public sealed class DuplicateCompositeParameterException : Exception
{
    internal DuplicateCompositeParameterException(MethodInfo methodInfo, ParameterInfo first, ParameterInfo second) :
        base(string.Format(
            Strings.DuplicateCompositeParameter,
            methodInfo.Name,
            methodInfo.DeclaringType!.Name,
            first.ParameterType,
            first.Name,
            second.Name)
        ) { }
}
