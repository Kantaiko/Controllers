using System.Collections.Immutable;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.EndpointMatching.Text;

/// <summary>
/// The property class that contains the text values of matched pattern.
/// </summary>
public sealed record TextMatchProperties : PropertyRecord<TextMatchProperties>
{
    /// <summary>
    /// The dictionary that maps the names of the matched parameters to their values.
    /// </summary>
    public IReadOnlyDictionary<string, string> Parameters { get; init; } = ImmutableDictionary<string, string>.Empty;
}
