using Api.Dtos;
using Application.Votes.Commands;
using MediatR;
using VotingApp.Api.Abstractions;

namespace Api.EndpointDefinitions
{
    public class Votes : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            var candidates = app.MapGroup("api/vote");

            candidates.MapPost("/submit", SubmitVote).WithName(nameof(SubmitVote));
        }

        private async Task<IResult> SubmitVote(IMediator mediator, VoteDto vote)
        {
            var result = await mediator.Send(new SubmitVote() { VoterId = vote.VoterId, CandidateId = vote.CandidateId });

            return string.IsNullOrEmpty(result.ResponseMessage)
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result.ResponseMessage);
        }
    }
}
