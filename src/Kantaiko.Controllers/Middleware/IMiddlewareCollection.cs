using System;
using System.Collections.Generic;

namespace Kantaiko.Controllers.Middleware
{
    public interface IMiddlewareCollection
    {
        IReadOnlyList<Type> MiddlewareTypes { get; }
    }
}
