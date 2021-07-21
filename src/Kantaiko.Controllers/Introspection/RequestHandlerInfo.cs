using System.Collections.Generic;

namespace Kantaiko.Controllers.Introspection
{
    public class RequestHandlerInfo
    {
        public RequestHandlerInfo(IReadOnlyList<ControllerInfo> controllers)
        {
            Controllers = controllers;
        }

        public IReadOnlyList<ControllerInfo> Controllers { get; }
    }
}
