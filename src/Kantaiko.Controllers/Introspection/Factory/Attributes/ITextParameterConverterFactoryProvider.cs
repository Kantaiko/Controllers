using System;
using Kantaiko.Controllers.Introspection.Factory.Context;
using Kantaiko.Controllers.ParameterConversion.Text;

namespace Kantaiko.Controllers.Introspection.Factory.Attributes;

public interface ITextParameterConverterFactoryProvider
{
    Func<IServiceProvider, ITextParameterConverter> GetTextParameterConverterFactory(ParameterFactoryContext context);
}
