using System.Collections.Generic;

namespace Kantaiko.Controllers.Design.Controllers
{
    public interface IControllerDesignPropertyProvider
    {
        IReadOnlyDictionary<string, object> GetControllerDesignProperties();
    }
}
