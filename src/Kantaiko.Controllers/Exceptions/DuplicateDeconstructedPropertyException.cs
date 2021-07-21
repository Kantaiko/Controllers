using System.Reflection;

namespace Kantaiko.Controllers.Exceptions
{
    public class DuplicateDeconstructedPropertyException : ControllersException
    {
        public DuplicateDeconstructedPropertyException(PropertyInfo first, PropertyInfo second) : base(
            $"Deconstructed parameter \"{first.DeclaringType!.Name}\" defines multiple properties with type " +
            $"\"{first.PropertyType}\": \"{first.Name}\" and \"{second.Name}\"") { }
    }
}
