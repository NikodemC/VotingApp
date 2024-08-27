using Application.Candidates.Commands;
using Application.Candidates.Queries;
using MediatR;
using VotingApp.Api.Abstractions;
using VotingApp.Api.Dtos;
using VotingApp.Api.Filters;

namespace VotingApp.Api.EndpointDefinitions
{
    public class Candidates : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            var candidates = app.MapGroup("api/candidates");

            candidates.MapGet("/{id}", GetCandidateById).WithName(nameof(GetCandidateById));
            candidates.MapGet("/", GetAllCandidates).WithName(nameof(GetAllCandidates));
            candidates.MapPost("/", AddCandidate).WithName(nameof(AddCandidate)).AddEndpointFilter<CandidateValidationFilter>();
        }

        private async Task<IResult> GetCandidateById(IMediator mediator, int id)
        {
            var candidate = await mediator.Send(new GetCandidateById() { Id = id });

            return candidate == null
                ? TypedResults.NotFound($"Candidate with id {id} not found.")
                : TypedResults.Ok(candidate);
        }

        private async Task<IResult> GetAllCandidates(IMediator mediator)
        {
            var candidates = await mediator.Send(new GetAllCandidates());

            return TypedResults.Ok(candidates);
        }

        private async Task<IResult> AddCandidate(IMediator mediator, CandidateDto candidateDto)
        {
            var createdCandidate = await mediator.Send(new AddCandidate() { Name = candidateDto.Name });
            return Results.CreatedAtRoute("GetCandidateById", new { createdCandidate.Id }, createdCandidate);
        }
    }
}
