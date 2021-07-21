using System.Reflection;

namespace Kantaiko.Controllers.Exceptions
{
    public class DuplicateParameterNameException : ControllersException
    {
        public DuplicateParameterNameException(MethodInfo methodInfo, string parameterName) : base(
            $"Endpoint \"{methodInfo.Name}\" defines multiple parameters with name \"{parameterName}\"") { }
    }
}
