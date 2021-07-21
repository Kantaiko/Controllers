using System.Reflection;
using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Middleware;

namespace Kantaiko.Controllers.Tests.Shared
{
    public class RequestHandlerProvider
    {
        public RequestHandler<TestRequest> RequestHandler { get; }

        public RequestHandlerProvider()
        {
            var controllerCollection = ControllerCollection.FromAssemblies(Assembly.GetExecutingAssembly());
            var converterCollection = ConverterCollection.FromAssemblies(Assembly.GetExecutingAssembly());
            var middlewareCollection = MiddlewareCollection.FromAssemblies(Assembly.GetExecutingAssembly());

            RequestHandler = new RequestHandler<TestRequest>(controllerCollection,
                converterCollection, middlewareCollection);
        }
    }
}
