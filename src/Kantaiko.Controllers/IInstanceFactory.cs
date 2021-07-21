using System;

namespace Kantaiko.Controllers
{
    public interface IInstanceFactory
    {
        object CreateInstance(Type type, IServiceProvider serviceProvider);
    }
}
