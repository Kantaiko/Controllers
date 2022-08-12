using System.Collections.Generic;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.Matching.Text;

public record MatchingTextParameterConversionProperties :
    ReadOnlyPropertiesBase<MatchingTextParameterConversionProperties>
{
    public IReadOnlyDictionary<string, string>? Parameters { get; init; }
}
