using System.Collections.Generic;
using System.Linq;
using Kantaiko.Properties;

namespace Kantaiko.Controllers.ParameterConversion.Validation;

public record PostValidationParameterProperties : ReadOnlyPropertiesBase<PostValidationParameterProperties>
{
    public IEnumerable<IParameterPostValidator> PostValidators { get; init; } =
        Enumerable.Empty<IParameterPostValidator>();
}
