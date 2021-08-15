using System;

namespace Kantaiko.Controllers.Exceptions
{
    public class InvalidDesignPropertyTypeException : ControllersException
    {
        public InvalidDesignPropertyTypeException(string key, Type expected, Type actual) : base(
            $"The design property with key \"{key}\" was expected to be of the \"{expected.FullName}\" type, " +
            $"but it was \"{actual.FullName}\"") { }
    }
}
