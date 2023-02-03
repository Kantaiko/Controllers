namespace Kantaiko.Controllers;

/// <summary>
/// Defines a mechanism for accepting a context object after controller creation.
/// </summary>
public interface IContextAcceptor
{
    /// <summary>
    /// Accepts a context object and stores it in the controller.
    /// </summary>
    /// <param name="context">The context object to accept.</param>
    void SetContext(object context);
}
