namespace Kantaiko.Controllers.Design.Parameters
{
    public interface IParameterDefaultValueResolverFactory
    {
        IParameterDefaultValueResolver CreateParameterDefaultValueResolver(EndpointParameterDesignContext context);
    }
}
