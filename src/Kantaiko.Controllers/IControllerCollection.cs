using System;
using System.Collections.Generic;

namespace Kantaiko.Controllers
{
    public interface IControllerCollection
    {
        IReadOnlyList<Type> ControllerTypes { get; }
    }
}
