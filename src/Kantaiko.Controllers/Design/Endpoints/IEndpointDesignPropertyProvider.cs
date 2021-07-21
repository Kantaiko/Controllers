using System.Collections.Generic;

namespace Kantaiko.Controllers.Design.Endpoints
{
    public interface IEndpointDesignPropertyProvider
    {
        IReadOnlyDictionary<string, object> GetEndpointDesignProperties();
    }
}
