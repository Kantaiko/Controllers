namespace Kantaiko.Controllers.Middleware
{
    public enum EndpointMiddlewareStage
    {
        BeforeExistenceCheck,
        BeforeValidation,
        BeforeResolution,
        BeforePostValidation,
        BeforeExecution
    }
}
