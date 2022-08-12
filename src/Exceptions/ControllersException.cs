using System;

namespace Kantaiko.Controllers.Exceptions;

public class ControllersException : Exception
{
    protected ControllersException(string message) : base(message) { }
}
