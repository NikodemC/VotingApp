using VotingApp.Api.Dtos;

namespace VotingApp.Api.Filters
{
    public class VoterValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var device = context.GetArgument<VoterDto>(1);
            if (string.IsNullOrEmpty(device.Name))
                return await Task.FromResult(Results.BadRequest("Name cannot be empty"));
            return await next(context);
        }
    }
}
