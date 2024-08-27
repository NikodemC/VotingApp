using Application.Voters.Commands;
using Application.Voters.Queries;
using MediatR;
using VotingApp.Api.Abstractions;
using VotingApp.Api.Dtos;
using VotingApp.Api.Filters;

namespace VotingApp.Api.EndpointDefinitions
{
    public class Voters : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            var voters = app.MapGroup("api/voters");

            voters.MapGet("/{id}", GetVoterById).WithName(nameof(GetVoterById));
            voters.MapGet("/", GetAllVoters).WithName(nameof(GetAllVoters));
            voters.MapPost("/", AddVoter).WithName(nameof(AddVoter)).AddEndpointFilter<VoterValidationFilter>();
        }

        private async Task<IResult> GetVoterById(IMediator mediator, int id)
        {
            var voter = await mediator.Send(new GetVoterById() { Id = id });

            return voter == null
                ? TypedResults.NotFound($"Voter with id {id} not found.")
                : TypedResults.Ok(voter);
        }

        private async Task<IResult> GetAllVoters(IMediator mediator)
        {
            var voters = await mediator.Send(new GetAllVoters());

            return TypedResults.Ok(voters);
        }

        private async Task<IResult> AddVoter(IMediator mediator, VoterDto voterDto)
        {
            var createdVoter = await mediator.Send(new AddVoter() { Name = voterDto.Name });
            return Results.CreatedAtRoute("GetVoterById", new { createdVoter.Id }, createdVoter);
        }
    }
}
