using System;
using Kantaiko.Controllers.Exceptions;

namespace Kantaiko.Controllers.ParameterConversion.Text.Exceptions;

public class ConverterNotFoundException : ControllersException
{
    public ConverterNotFoundException(Type type) : base($"Converter for the type \"{type}\" was not found") { }
}
