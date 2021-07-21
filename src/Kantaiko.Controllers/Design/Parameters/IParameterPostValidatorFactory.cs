using Kantaiko.Controllers.Validation;

namespace Kantaiko.Controllers.Design.Parameters
{
    public interface IParameterPostValidatorFactory
    {
        IParameterPostValidator CreateParameterPostValidator(EndpointParameterDesignContext context);
    }
}
