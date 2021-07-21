using System;

namespace Kantaiko.Controllers.Exceptions
{
    public class ControllersException : Exception
    {
        public ControllersException(string message) : base(message) { }
    }
}
