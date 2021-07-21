using System;
using Kantaiko.Controllers.Introspection;

namespace Kantaiko.Controllers.Design.Controllers
{
    public class ControllerDesignContext
    {
        public ControllerDesignContext(ControllerInfo info, IServiceProvider serviceProvider)
        {
            Info = info;
            ServiceProvider = serviceProvider;
        }

        public ControllerInfo Info { get; }
        public IServiceProvider ServiceProvider { get; }
    }
}
