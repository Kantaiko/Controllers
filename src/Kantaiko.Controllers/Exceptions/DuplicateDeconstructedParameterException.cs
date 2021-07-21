using System.Reflection;

namespace Kantaiko.Controllers.Exceptions
{
    public class DuplicateDeconstructedParameterException : ControllersException
    {
        public DuplicateDeconstructedParameterException(MethodInfo methodInfo, ParameterInfo first,
            ParameterInfo second) : base(
            $"Endpoint \"{methodInfo.Name}\" of controller \"{methodInfo.DeclaringType!.Name}\" defines " +
            $"multiple deconstructed parameters of type \"{first.ParameterType}\": " +
            $"\"{first.Name}\" and \"{second.Name}\"") { }
    }
}
