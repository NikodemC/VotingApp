namespace VotingApp.Api.Abstractions
{
    public interface IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app);
    }
}
