using System.Collections.Generic;

namespace Kantaiko.Controllers.Design.Parameters
{
    public interface IParameterDesignPropertyProvider
    {
        IReadOnlyDictionary<string, object> GetParameterDesignProperties();
    }
}
