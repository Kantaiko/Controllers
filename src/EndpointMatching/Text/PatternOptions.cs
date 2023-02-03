namespace Kantaiko.Controllers.EndpointMatching.Text;

/// <summary>
/// The options for the text pattern matching.
/// </summary>
[Flags]
public enum PatternOptions
{
    /// <summary>
    /// No options.
    /// </summary>
    None = 0,

    /// <summary>
    /// Ignore case when matching.
    /// </summary>
    IgnoreCase = 1 << 0,

    /// <summary>
    /// Allow matching multiple lines.
    /// </summary>
    MultiLine = 1 << 1,

    /// <summary>
    /// Require the pattern to match the whole text.
    /// </summary>
    CompleteMatch = 1 << 2,
}
