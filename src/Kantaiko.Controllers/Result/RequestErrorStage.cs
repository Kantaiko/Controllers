namespace Kantaiko.Controllers.Result
{
    public enum RequestErrorStage
    {
        EndpointMatching,
        ParameterExistenceCheck,
        ParameterValidation,
        ParameterResolution,
        ParameterPostValidation,
        EndpointInvocation
    }
}
