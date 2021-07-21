using System;

namespace Kantaiko.Controllers.Exceptions
{
    public class ConverterNotFoundException : ControllersException
    {
        public ConverterNotFoundException(Type type) : base($"Converter for the type \"{type}\" was not found") { }
    }
}
