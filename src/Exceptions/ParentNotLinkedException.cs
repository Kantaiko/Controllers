using Kantaiko.Controllers.Resources;

namespace Kantaiko.Controllers.Exceptions;

/// <summary>
/// The exception that is thrown when trying to access a parent of a non-linked controller, endpoint or parameter.
/// </summary>
public sealed class ParentNotLinkedException : Exception
{
    internal ParentNotLinkedException(object instance) :
        base(string.Format(Strings.ParentNotLinked, instance.GetType().Name)) { }
}
