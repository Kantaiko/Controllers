using System;

namespace Kantaiko.Controllers.Introspection.Factory.Transformers;

public interface IIntrospectionInfoTransformer
{
    IntrospectionInfo Transform(IntrospectionInfo introspectionInfo, IServiceProvider serviceProvider);
}
